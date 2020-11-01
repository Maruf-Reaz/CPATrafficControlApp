using System;
using CPATCMSApp.Models.CnFs;
using CPATCMSApp.Models.Yards;
using CPATCMSApp.Models.Gates;
using CPATCMSApp.Models.Currency;
using CPATCMSApp.Models.Assignments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Models.TrialAssignments;
using CPATCMSApp.Models.Common.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CPATCMSApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserType>().HasData(
                new UserType() { Id = 1, Name = "CNF" },
                new UserType() { Id = 2, Name = "Gate" },
                new UserType() { Id = 3, Name = "Gate Admin" },
                new UserType() { Id = 4, Name = "Yard" },
                new UserType() { Id = 5, Name = "One Stop" },
                new UserType() { Id = 6, Name = "Mechanical" },
                new UserType() { Id = 7, Name = "TM Office" },
                new UserType() { Id = 8, Name = "Admin" },
                new UserType() { Id = 9, Name = "PaymentAdmin" },
                new UserType() { Id = 10, Name = "SuperAdmin" }
              );

            modelBuilder.Entity<AssignmentSlot>().HasData(
                new AssignmentSlot() { Id = 1, AssignmentName = "Assignment 1 (5 PM)", StartTime = 1001, EndTime = 1700 },
                new AssignmentSlot() { Id = 2, AssignmentName = "Assignment 2 (9 PM)", StartTime = 1701, EndTime = 2100 },
                new AssignmentSlot() { Id = 3, AssignmentName = "Assignment 3 (10 AM)", StartTime = 2101, EndTime = 1000 },
                new AssignmentSlot() { Id = 4, AssignmentName = "Assignment 4 (Special)", StartTime = 1400, EndTime = 1600 }
              );

            modelBuilder.Entity<Bay>().HasData(
                new Bay() { Id = 1, Name = "GCB" }
              );

            modelBuilder.Entity<Yard>().HasData(
                new Yard() { Id = 1, Name = "Yard 1", Number = "1", GateId = 1, BayId = 1 },
                new Yard() { Id = 2, Name = "Yard 2", Number = "2", GateId = 2, BayId = 1 }
              );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType() { Id = 1, Name = "Recharge" },
                new PaymentType() { Id = 2, Name = "Revenue" },
                new PaymentType() { Id = 3, Name = "Fine" }
              );

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod() { Id = 1, Name = "bKash" },
                new PaymentMethod() { Id = 2, Name = "Rocket" },
                new PaymentMethod() { Id = 3, Name = "Nagad" },
                new PaymentMethod() { Id = 4, Name = "System" }
             );

            modelBuilder.Entity<Gate>().HasData(
                new Gate() { Id = 1, Name = "Gate 1" },
                new Gate() { Id = 2, Name = "Gate 2" },
                new Gate() { Id = 3, Name = "Gate 3" }
             );

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = "a682b56a-6135-4111-a0k0-bdec547e3waz", Name = "HarbourAndMarine", NormalizedName = "HARBOURANDMARINE", ConcurrencyStamp = "da9a3b0e-8b6f-8529-71d0-4fd255e23f15", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has All Permissions" },
                new ApplicationRole { Id = "b793b57a-6135-4221-b0l0-bdec547e3wax", Name = "SuperAdmin", NormalizedName = "SUPERADMIN", ConcurrencyStamp = "ga9a3b0e-8b6f-9130-67d0-4fd255e23f16", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has All Permissions" },
                new ApplicationRole { Id = "c814b58b-7456-9332-c0m0-bdec765e3awc", Name = "Cnf", NormalizedName = "CNF", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f16", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "d925b59b-7456-1442-d0n0-bdec765e3awv", Name = "GateSergent", NormalizedName = "GATESERGENT", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f17", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "e136b60b-7456-2552-e0o0-bdec765e3awb", Name = "GateAdmin", NormalizedName = "GATEADMIN", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f18", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "f247b61b-7456-3662-f0p0-bdec765e3awn", Name = "Yard", NormalizedName = "YARD", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f19", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "g358b62b-7456-6772-g0q0-bdec765e3awm", Name = "OneStop", NormalizedName = "ONESTOP", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f20", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "h469b63b-7456-7882-h0r0-bdec765e3awl", Name = "Mechanical", NormalizedName = "MECHANICAL", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f21", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Minimum Permissions" },
                new ApplicationRole { Id = "i571b64b-7456-5992-i0s0-bdec765e3awk", Name = "TMOffice", NormalizedName = "TMOFFICE", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f22", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Viewer Permissions" },
                new ApplicationRole { Id = "j693b65b-7456-8112-j0t0-bdec765e3awj", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "ea9a3b0f-9b5f-7153-81e0-4fd799e23f23", CreatedOn = Convert.ToDateTime("2020-01-01"), Description = "Has Viewer Permissions" }
             );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser() { Id = "8ab6ee61-f36c-41b1-ae27-dbb23cbfb507", UserName = "HarbourAndMArine", NormalizedUserName = "HARBOURANDMARINE", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EHJ", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2d", YardId = null, GateId = null, UserTypeId = 8 },
                new ApplicationUser() { Id = "8ab6ee62-f37c-42b2-ae27-dbb11cbfb508", UserName = "NCYGateIn", NormalizedUserName = "NCYGATEIN", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EHK", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2e", YardId = null, GateId = 1, UserTypeId = 2 },
                new ApplicationUser() { Id = "8ab6ee63-f38c-43b3-ae27-dbb22cbfb509", UserName = "NCYGateOut", NormalizedUserName = "NCYGATEOUT", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPL", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2f", YardId = null, GateId = 2, UserTypeId = 2 },
                new ApplicationUser() { Id = "8ab6ee64-f39c-44b4-ae27-dbb33cbfb510", UserName = "GateAdmin", NormalizedUserName = "GATEADMIN", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPM", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2g", YardId = null, GateId = null, UserTypeId = 3 },
                new ApplicationUser() { Id = "8ab6ee65-f40c-45b5-ae27-dbb44cbfb511", UserName = "YardAdmin", NormalizedUserName = "YARDADMIN", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPN", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2h", YardId = 1, GateId = null, UserTypeId = 4 },
                new ApplicationUser() { Id = "8ab6ee66-f41c-46b6-ae27-dbb55cbfb512", UserName = "OneStop", NormalizedUserName = "ONESTOP", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPP", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2j", YardId = null, GateId = null, UserTypeId = 5 },
                new ApplicationUser() { Id = "8ab6ee67-f42c-47b7-ae27-dbb66cbfb513", UserName = "Mechanical", NormalizedUserName = "MECHANICAL", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPQ", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2k", YardId = null, GateId = null, UserTypeId = 6 },
                new ApplicationUser() { Id = "8ab6ee68-f43c-48b8-ae27-dbb77cbfb514", UserName = "TMOffice", NormalizedUserName = "TMOFFICE", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EPR", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2l", YardId = null, GateId = null, UserTypeId = 7 },
                new ApplicationUser() { Id = "8ab6ee69-f44c-49b9-ae27-dbb88cbfb515", UserName = "Admin", NormalizedUserName = "ADMIN", Email = "test@mail.com", NormalizedEmail = "TEST@MAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "5TDMS5CNA2GYJK2URB4GDOCQI2NI7EP3", ConcurrencyStamp = "26d21881-0a3a-44ab-aa4d-10664ace1e2m", YardId = null, GateId = null, UserTypeId = 9 },
                new ApplicationUser() { Id = "8ac6ee70-f45c-50b0-ae27-dbb99cbfb516", UserName = "NeuroStorm", NormalizedUserName = "NEUROSTORM", Email = "neurostorm.soft@gmail.com", NormalizedEmail = "NEUROSTORM.SOFT@GMAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEOJVsHUa611Khzkcg/zXgZ8EeegKhZAyW2eVPMzWJiToPuR45aBwuID99TNJ91JPxg==", SecurityStamp = "6VCMS5CNA2GYJI4NET6YGDOCQI2NI7EP4", ConcurrencyStamp = "26d21779-1a4a-55ab-aa4d-34567ace1e2n", YardId = null, GateId = null, UserTypeId = 10 }
             );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = "8ab6ee61-f36c-41b1-ae27-dbb23cbfb507", RoleId = "a682b56a-6135-4111-a0k0-bdec547e3waz" },
                new IdentityUserRole<string>() { UserId = "8ab6ee62-f37c-42b2-ae27-dbb11cbfb508", RoleId = "d925b59b-7456-1442-d0n0-bdec765e3awv" },
                new IdentityUserRole<string>() { UserId = "8ab6ee63-f38c-43b3-ae27-dbb22cbfb509", RoleId = "d925b59b-7456-1442-d0n0-bdec765e3awv" },
                new IdentityUserRole<string>() { UserId = "8ab6ee64-f39c-44b4-ae27-dbb33cbfb510", RoleId = "e136b60b-7456-2552-e0o0-bdec765e3awb" },
                new IdentityUserRole<string>() { UserId = "8ab6ee65-f40c-45b5-ae27-dbb44cbfb511", RoleId = "f247b61b-7456-3662-f0p0-bdec765e3awn" },
                new IdentityUserRole<string>() { UserId = "8ab6ee66-f41c-46b6-ae27-dbb55cbfb512", RoleId = "g358b62b-7456-6772-g0q0-bdec765e3awm" },
                new IdentityUserRole<string>() { UserId = "8ab6ee67-f42c-47b7-ae27-dbb66cbfb513", RoleId = "h469b63b-7456-7882-h0r0-bdec765e3awl" },
                new IdentityUserRole<string>() { UserId = "8ab6ee68-f43c-48b8-ae27-dbb77cbfb514", RoleId = "i571b64b-7456-5992-i0s0-bdec765e3awk" },
                new IdentityUserRole<string>() { UserId = "8ab6ee69-f44c-49b9-ae27-dbb88cbfb515", RoleId = "j693b65b-7456-8112-j0t0-bdec765e3awj" },
                new IdentityUserRole<string>() { UserId = "8ac6ee70-f45c-50b0-ae27-dbb99cbfb516", RoleId = "b793b57a-6135-4221-b0l0-bdec547e3wax" }
             );

        }

        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Bay> Bays { get; set; }
        public DbSet<Yard> Yards { get; set; }
        public DbSet<AssignmentSlot> AssignmentSlots { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentItem> AssignmentItems { get; set; }
        public DbSet<TruckEntity> TruckEntities { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<CnFProfile> CnFProfiles { get; set; }
        public DbSet<CnFRegistration> CnFRegistrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<BkashMessage> BkashMessages { get; set; }

        //New Assignment Model
        public DbSet<TrialAssignmentItem> TrialAssignmentItems { get; set; }
    }
}
