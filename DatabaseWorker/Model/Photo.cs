//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseWorker.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public string ImageLink { get; set; }
        public int Id { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
