// <auto-generated />
namespace Data.Npgsql.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class addedforeignkeycontraintbetweenshiftandschedule : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(addedforeignkeycontraintbetweenshiftandschedule));
        
        string IMigrationMetadata.Id
        {
            get { return "201611051942168_added foreign key contraint between shift and schedule"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}