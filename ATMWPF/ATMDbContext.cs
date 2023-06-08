using System;
using ATMLib;
using Microsoft.EntityFrameworkCore;

namespace ATMWPF
{
    public class ATMDbContext : DbContext
    {
        public ATMDbContext()
        {
            var dbExists = Database.EnsureCreated();
            if (dbExists) 
            { 
                GenerateSmthng();
            }
        }

        private void GenerateSmthng()
        {
            var account = new Account
            {
                CardNumber = "5168755907057703",
                CardPIN = "1234",
                Name = "Maksym",
                Surname = "Mahaz",
                Balance = 1000,
                CardType = CardType.Mastercard
            };

            Accounts.Add(account);

            var ATM = new ATM
            {
                ATMBalance = 5000,
                Adress = "Prostpekt mira",
                Bank = new Bank
                {
                    Name = "PrivatBank",
                    SupportPhone = "+380931359244"
                }
            };

            ATMs.Add(ATM);


            Banks.Add(ATM.Bank);
            SaveChanges();
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<ATM> ATMs { get; set; }

        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ATM>().HasOne(a => a.Bank).WithMany(b => b.ATMList).HasForeignKey(a => a.BankId);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString:
               "Server=localhost;Port=5432;User Id=postgres;Password=159874;Database=ATMDb;");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnConfiguring(optionsBuilder);
        }

       
    }

}

