//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.SqlDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Words: IWord
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        public int ItemId { get; set; }
    
        public virtual Items Item { get; set; }
    }
}
