//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLySoTietKiem.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOTIETKIEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SOTIETKIEM()
        {
            this.PHIEUGOITIENs = new HashSet<PHIEUGOITIEN>();
            this.PHIEURUTTIENs = new HashSet<PHIEURUTTIEN>();
        }
    
        public int MaSo { get; set; }
        public int MaLoaiTietKiem { get; set; }
        public int MaKhachHang { get; set; }
        public Nullable<System.DateTime> NgayMoSo { get; set; }
        public Nullable<int> SoTienGoi { get; set; }
        public Nullable<bool> BiDong { get; set; }
        public Nullable<System.DateTime> NgayTinhLaiGanNhat { get; set; }
        public Nullable<decimal> LaiSuat { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual LOAITIETKIEM LOAITIETKIEM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUGOITIEN> PHIEUGOITIENs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEURUTTIEN> PHIEURUTTIENs { get; set; }
    }
}
