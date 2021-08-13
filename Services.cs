using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace H4BankVault
{
    public class Services
    {
        const string keyPath = "C:\\Users\\alla1862\\Documents\\";
        const string jsonPath = "C:\\Users\\alla1862\\source\\repos\\H4BankVault\\Accounts.json";
        //const string keyPath = "C:\\Users\\Kiror_Avaldea\\Documents\\";
        //const string jsonPath = "C:\\Users\\Kiror_Avaldea\\Source\\Repos\\H4BankVault\\Accounts.json";

        public List<Account> Accounts { get; set; }

        public Services()
        {
            Accounts = new List<Account>();
            if (File.Exists(jsonPath))
            {
                string temp = File.ReadAllText(jsonPath);
                if (temp != null && temp != "")
                {
                    Accounts = JsonConvert.DeserializeObject<List<Account>>(temp).OrderBy(a => a.AccountId).ToList();
                }
            }
        }

        public Account CreateAccount(string password, string note)
        {
            AesEncryption aes = new AesEncryption();
            RSAWithXML rsa = new RSAWithXML();
            byte[] aES_Key_temp = aes.GenerateRandomNumber(32), iV = aes.GenerateRandomNumber(16);


            Account account = new Account();
            if (Accounts.Count() >= 1)
            {
                account.AccountId = Accounts.LastOrDefault().AccountId + 1;
            }
            else
            {
                account.AccountId = 1;
            }
            account.Private_Key_Path = $"{keyPath}PriateKey_{account.AccountId}";
            account.Public_Key_Path = $"{keyPath}PublicKey_{account.AccountId}";
            rsa.AssignNewKey(account.Public_Key_Path, account.Private_Key_Path);
            account.AES_Key = Convert.ToBase64String(rsa.EncryptData(account.Public_Key_Path, aES_Key_temp));
            account.IV = Convert.ToBase64String(rsa.EncryptData(account.Public_Key_Path, iV));
            account.Note = Convert.ToBase64String(aes.Encrypt(Encoding.UTF8.GetBytes(note), aES_Key_temp, iV));
            account.Salt = Cryptography.GenerateSalt();
            account.Hash = Cryptography.CreateHashedPassword(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(account.Salt));
            account.Banlance = 0;

            Accounts.Add(account);
            return account;
        }

        public void SaveAccounts()
        {
            string json = JsonConvert.SerializeObject(Accounts, Formatting.Indented);
            if (File.Exists(jsonPath))
            {
                File.WriteAllText(jsonPath, json);
            }
            else
            {
                File.Create(jsonPath);
                File.WriteAllText(jsonPath, json);
            }
        }

        public Account GetAccount(int accountId, string password)
        {
            Account account = Accounts.SingleOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                if (account.Hash == Cryptography.CreateHashedPassword(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(account.Salt)))
                {
                    return account;
                }
            }
            throw new Exception("Kunne ikke finde brugeren");
        }

        public Account AccountNote(Account account, string note)
        {
            AesEncryption aes = new AesEncryption();
            RSAWithXML rsa = new RSAWithXML();
            byte[] aES_Key_temp = aes.GenerateRandomNumber(32), iV = aes.GenerateRandomNumber(16);

            account = Accounts.SingleOrDefault(a => a.AccountId == account.AccountId);

            account.AES_Key = Convert.ToBase64String(rsa.EncryptData(account.Public_Key_Path, aES_Key_temp));
            account.IV = Convert.ToBase64String(rsa.EncryptData(account.Public_Key_Path, iV));
            account.Note = Convert.ToBase64String(aes.Encrypt(Encoding.UTF8.GetBytes(note), aES_Key_temp, iV));

            return account;
        }

        public string GetNote(Account account)
        {
            AesEncryption aes = new AesEncryption();
            RSAWithXML rsa = new RSAWithXML();
            byte[] aeskey = Convert.FromBase64String(account.AES_Key), iV = Convert.FromBase64String(account.IV);
            aeskey = rsa.DecryptData(account.Private_Key_Path, aeskey);
            iV = rsa.DecryptData(account.Private_Key_Path, iV);
            return Encoding.UTF8.GetString(aes.Decrypt(Convert.FromBase64String(account.Note), aeskey, iV));
        }

        public void DeleteAccount(Account account)
        {
            File.Delete(account.Private_Key_Path);
            File.Delete(account.Public_Key_Path);
            Accounts.Remove(account);
        }
    }
}
