//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Key { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<byte> IsOnline { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
    }
}