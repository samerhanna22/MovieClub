namespace MovieClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {

            // admin pass : Admin@2018

            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'8f2ddc9b-2a24-4e17-b864-5bf8d14c8e16', N'guest@MovieClub.com', 0, N'AJV3oD69Hxy+HHwaqMEj/l1Q+NCo4NVuEcfY3S0lcwPLR2FH+Nf38uKO7BoYrqeAfA==', N'249346ef-9902-4407-b5cd-5f18459befe3', NULL, 0, 0, NULL, 1, 0, N'guest@MovieClub.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'e88470fc-103b-46cd-b7a2-8e29c15e5f2a', N'admin@MovieClub.com', 0, N'APlwU5ycviOMtr8VFWvBd7gv9gyijQq6INyUo/CnYiWul/ztNZWKDibWcuoSxa7USA==', N'd7eae983-4406-4f87-91f8-2b28fbd42563', NULL, 0, 0, NULL, 1, 0, N'admin@MovieClub.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'f43d0756-e5ed-475f-b75c-0d9f2a82609c', N'CanManageMovies')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e88470fc-103b-46cd-b7a2-8e29c15e5f2a', N'f43d0756-e5ed-475f-b75c-0d9f2a82609c')


");

        }

        public override void Down()
        {
        }
    }
}
