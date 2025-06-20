using BDOExamAPI.Application.Interfaces;
using BDOExamAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDOExamAPI.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionLog> TransactionLog => Set<TransactionLog>();
        public DbSet<AccountNumberSequence> AccountNumberSequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber).IsUnique();
                entity.Property(e => e.Balance).HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
                entity.HasCheckConstraint("CK_Account_AccountType", "[AccountType] IN ('Savings', 'Checking')");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("getdate()");
                entity.HasCheckConstraint("CK_Transaction_TransactionType", "[TransactionType] IN ('Deposit', 'Withdrawal', 'Transfer')");
            });

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.FromTransactions)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.ToTransactions)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);


            // transaction log
            modelBuilder.Entity<TransactionLog>(entity =>
            {
                entity.ToTable("TransactionLog");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Operation)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,4)");

                entity.Property(e => e.OldBalance)
                    .HasColumnType("decimal(18,4)");

                entity.Property(e => e.NewBalance)
                    .HasColumnType("decimal(18,4)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.HasOne(e => e.Account)
                    .WithMany()
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Transaction)
                    .WithMany()
                    .HasForeignKey(e => e.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            //seq
            modelBuilder.Entity<AccountNumberSequence>(entity =>
            {
                entity.HasKey(e => e.Id);
                 
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();
                 
                entity.HasIndex(e => e.SequenceDate)
                      .IsUnique();
                 
                entity.Property(e => e.SequenceDate)
                      .HasMaxLength(6)
                      .IsRequired();
            });
        }
    }
}
