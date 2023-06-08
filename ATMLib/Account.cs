namespace ATMLib
{
    public enum CardType
    {
        Mastercard = 0,
        Visa
    }

    public class Account
    {
        public long Id { get; set; }

        public string CardNumber { get; set; }

        public string CardPIN { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public decimal Balance { get; set; }

        public CardType CardType { get; set; }
    }
}