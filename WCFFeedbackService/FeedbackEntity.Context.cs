﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFFeedbackService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FeedbackEntities : DbContext
    {
        public FeedbackEntities()
            : base("name=FeedbackEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<TCourse> TCourse { get; set; }
        public DbSet<TStudent> TStudent { get; set; }
        public DbSet<vwCourses> vwCourses { get; set; }
        public DbSet<vwStudentCourse> vwStudentCourse { get; set; }
        public DbSet<TStudentCourse> TStudentCourse { get; set; }
        public DbSet<TFeedback> TFeedback { get; set; }
        public DbSet<vwFeedbacks> vwFeedbacks { get; set; }
    }
}
