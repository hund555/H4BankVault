namespace H4BankVault
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Private_Key_Path { get; set; }
        public string Public_Key_Path { get; set; }
        public string AES_Key { get; set; }
        public string IV { get; set; }
        public string Note { get; set; }
        public double Banlance { get; set; }
    }
}
