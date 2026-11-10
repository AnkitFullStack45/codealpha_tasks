using Circlely.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Circlely.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMember> ConversationMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CallHistory> CallHistories { get; set; }
        public DbSet<Story> Stories { get; set; }

       protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // =========================================================
    // ConversationMember
    // =========================================================

    builder.Entity<ConversationMember>()
        .HasKey(x => new
        {
            x.ConversationId,
            x.UserId
        });

    builder.Entity<ConversationMember>()
        .HasOne(x => x.Conversation)
        .WithMany(x => x.Members)
        .HasForeignKey(x => x.ConversationId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<ConversationMember>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);


    // =========================================================
    // Post
    // =========================================================

    builder.Entity<Post>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);


    // =========================================================
    // Comment
    // =========================================================

    builder.Entity<Comment>()
        .HasOne(x => x.Post)
        .WithMany(x => x.Comments)
        .HasForeignKey(x => x.PostId)
        .OnDelete(DeleteBehavior.NoAction);

    builder.Entity<Comment>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);


    // =========================================================
    // Like
    // =========================================================

    builder.Entity<Like>()
        .HasOne(x => x.Post)
        .WithMany(x => x.Likes)
        .HasForeignKey(x => x.PostId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<Like>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    // Prevent duplicate likes
    builder.Entity<Like>()
        .HasIndex(x => new
        {
            x.PostId,
            x.UserId
        })
        .IsUnique();


    // =========================================================
    // Follow
    // =========================================================

    builder.Entity<Follow>()
        .HasOne(x => x.Follower)
        .WithMany()
        .HasForeignKey(x => x.FollowerId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Follow>()
        .HasOne(x => x.Following)
        .WithMany()
        .HasForeignKey(x => x.FollowingId)
        .OnDelete(DeleteBehavior.Restrict);

    // Prevent duplicate follows
    builder.Entity<Follow>()
        .HasIndex(x => new
        {
            x.FollowerId,
            x.FollowingId
        })
        .IsUnique();


    // =========================================================
    // Message
    // =========================================================

    builder.Entity<Message>()
        .HasOne(x => x.Conversation)
        .WithMany(x => x.Messages)
        .HasForeignKey(x => x.ConversationId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<Message>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.SenderId)
        .OnDelete(DeleteBehavior.Restrict);


    // =========================================================
    // CallHistory
    // =========================================================

    builder.Entity<CallHistory>()
        .HasOne(x => x.Caller)
        .WithMany()
        .HasForeignKey(x => x.CallerId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<CallHistory>()
        .HasOne(x => x.Reciever)
        .WithMany()
        .HasForeignKey(x => x.RecieverId)
        .OnDelete(DeleteBehavior.Restrict);


    // =========================================================
    // Story
    // =========================================================

    builder.Entity<Story>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);
            // =========================================================
            // FollowRequest
            // =========================================================

            builder.Entity<FollowRequest>()
                .HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FollowRequest>()
                .HasOne(x => x.Receiver)
                .WithMany()
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate pending requests
            builder.Entity<FollowRequest>()
                .HasIndex(x => new
                {
                    x.SenderId,
                    x.ReceiverId
                })
                .IsUnique();
        }
    }
}