# Bank Vault

## Discription
Du skal lave en Vault eller "bankboks", der gør det muligt at opbevare personlige dokumenter, passwords og andet på en sikker måde så kun personer med korrekt key kan få adgang.

Formålet er at træne emnerne Hashing og Encryption.

Man kan vælge forskellige scenarier, som f.eks.:

- En desktop applikation (Console, WPF eller Winforms), som kræver et password for at dekryptere en fil.
- Eller man kan lave en webapp, som kan gemme ens password på en sikker måde. Derved kræver blot at man skal huske et eneste password.
- Man kan også lægge vægten på Signing af emails og demonstrere en løsning der både signer og krypterer mails i f.eks. Outlook.
- Der er også mulighed for selv at formulere et oplæg inden for rammerne af faget.

## Krav

#### Data som skal gemmes

- [x] Konto ID
- [x] Hash
- [x] Salt
- [x] RSA Private key (gemt som xml)
- [x] RSA Public key (gemt som xml)
- [x] AES key (Krypteret med RSA)
- [x] IV (Krypteret med RSA)
- [x] Konto note (Krypteret med Aes)
- [x] Konto beløb

#### Funktioner

- [x] Hashing af kode med salt

    ```c#
        /// <summary>
        /// Hash password using a bytearray of the input, the salt and iterate 5000 times through the process.
        /// </summary>
        /// <param name="inputToHash">Input parameter to be hashed</param>
        /// <param name="salt">Salt for the hashing</param>
        /// <returns>Hashed password</returns>

        public static string CreateHashedPassword(byte[] inputToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(inputToHash, salt, 5000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        /// <summary>
        /// Create a byte array and fill it with random data using the randomizer called RNGCryptoServiceProvider.
        /// </summary>
        /// <returns>Salt</returns>
        public static string GenerateSalt()
        {
            var saltArray = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(saltArray);
            return Convert.ToBase64String(saltArray);
        }
    ```
- [x] Login
    ```c#
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
    ```
- [x] Oprettelse af bruger
    ```c#
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
    ```
- [x] Kryptering
    ```c#
        public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                //Ikke nødvendige at sætte på da de er på som default
                //aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }
    ```
- [x] Dekryptering
    ```c#
        public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                //Ikke nødvendige at sætte på da de er på som default
                //aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptBytes = memoryStream.ToArray();

                    return decryptBytes;
                }
            }
        }
    ```

- [x] Gemt i json
- [x] Hente en krypteret note og vise den dekrypteret
- [x] Slette en bruger
#### Ekstra

- [ ] Ændring af kode med ny salt
- [ ] Signing
- [ ] Certificat