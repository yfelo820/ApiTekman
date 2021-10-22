using Api.Databases.Content.Configurations;
using Api.Entities.Content;
using Microsoft.EntityFrameworkCore;

namespace Api.Databases.Content
{	
	public class ContentDbContext: DbContext
	{
		public ContentDbContext(DbContextOptions<ContentDbContext> options, bool isTest = false)
			:base(options)
		{
			if(!isTest) Database.SetCommandTimeout(300);
		}

		public DbSet<Course> Course { get; set; }
        public DbSet<ContentBlock> ContentBlock { get; set; }
        public DbSet<SuperContentBlock> SuperContentBlock { get; set; }
        public DbSet<Activity> Activity { get; set; }
		public DbSet<Subject> Subject { get; set; }
		public DbSet<Scene> Scene { get; set; }
		public DbSet<Transition> Transition { get; set; }
		public DbSet<TransitionProperty> TransitionProperty { get; set; }
		public DbSet<Exercise> Exercise { get; set; }
		public DbSet<Template> Template { get; set; }
		public DbSet<Feedback> Feedback { get; set; }
		public DbSet<Item> Item { get; set; }
		public DbSet<ItemDrag> ItemDrag { get; set; }
		public DbSet<ItemDrop> ItemDrop { get; set; }
		public DbSet<DragDrop> DragDrop { get; set; }
		public DbSet<ItemInput> ItemInput { get; set; }
		public DbSet<ItemSelect> ItemSelect { get; set; }
		public DbSet<ItemDraw> ItemDraw { get; set; }
		public DbSet<ItemStatic> ItemStatic { get; set; }
		public DbSet<ItemSelectGroup> ItemSelectGroup { get; set; }
		public DbSet<Multimedia> Multimedia { get; set; }
		public DbSet<Style> Style { get; set; }
		public DbSet<Language> Language { get; set; }
		public DbSet<Achievement> Achievement { get; set; }
		public DbSet<Log> Log { get; set; }
        public DbSet<ProblemResolution> ProblemResolution { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.ApplyConfiguration(new MultimediaConfiguration());
            modelBuilder.Entity<DragDrop>()
				.HasIndex(a => new { a.ItemDragId, a.ItemDropId })
				.HasName("Index_DragDrop_UniqueDragDrop")
				.IsUnique();
			modelBuilder.Entity<TransitionProperty>()
				.HasIndex(a => new { a.ItemState, a.ItemType, a.Property, a.TransitionId })
				.HasName("Index_TransitionProperty_UniqueTransitionProp")
				.IsUnique();
			modelBuilder.Entity<ContentBlock>()
				.HasIndex(a => new { a.Order, a.LanguageId, a.SubjectId })
				.HasName("Index_ContentBlock_UniqueOrderForLanguageSubject")
				.IsUnique();
            modelBuilder.Entity<Item>()
                .Property(a => a.MediaType)
                .HasDefaultValue(MediaType.Image);
            modelBuilder.Entity<Item>()
                .Property(a => a.AudioPlayerType)
                .HasDefaultValue(AudioPlayerType.Button);
            
        }
	}
}
