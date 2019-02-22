using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.Net.Attributes.ColumnAttribute;


namespace 壁纸UWP.Models
{
    [Table("Config")]
    class UserItem
    {/// <summary>
     /// FilePath存放文件路径
     /// </summary>
        [Column("FilePath")]
        [NotNull]
        public string FilePath { get; set; }
    }
}
