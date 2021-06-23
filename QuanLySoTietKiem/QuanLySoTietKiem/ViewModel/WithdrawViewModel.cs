﻿using QuanLySoTietKiem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace QuanLySoTietKiem.ViewModel
{
    public class WithdrawViewModel: BaseViewModel
    {
        private ObservableCollection<PHIEURUTTIEN> _List;
        public ObservableCollection<PHIEURUTTIEN> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private ObservableCollection<PHIEURUTTIEN> _Init;
        public ObservableCollection<PHIEURUTTIEN> Init { get => _Init; set { _Init = value; OnPropertyChanged(); } }
        private ObservableCollection<SOTIETKIEM> _ListSTK;
        public ObservableCollection<SOTIETKIEM> ListSTK { get => _ListSTK; set { _ListSTK = value; OnPropertyChanged(); } }
        private string _TenKhachHang;
        public string TenKhachHang { get => _TenKhachHang; set { _TenKhachHang = value; OnPropertyChanged(); } }
        private string _MaSo;
        public string MaSo { get => _MaSo; set { _MaSo = value; CheckMaSTK(_MaSo); OnPropertyChanged(); } }
        private string _SoTienRut;
        public string SoTienRut { get => _SoTienRut; set { _SoTienRut = value; checkTienRut(MaSo); OnPropertyChanged(); } }
        private string _SoTienGui;
        public string SoTienGui { get => _SoTienGui; set { _SoTienGui = value; OnPropertyChanged(); } }
        private string _LoaiSTK;
        public string LoaiSTK { get => _LoaiSTK; set { _LoaiSTK = value; OnPropertyChanged(); } }

        private string _CMND;
        public string CMND { get => _CMND; set { _CMND = value; OnPropertyChanged(); } }
        private String[] _FilterList;
        public String[] FilterList { get => _FilterList; set { _FilterList = value; OnPropertyChanged(); } }
        private PHIEURUTTIEN _SelectedItem;
        public PHIEURUTTIEN SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(); } }
        private string _SelectedFilter;
        public string SelectedFilter { get => _SelectedFilter; set { _SelectedFilter = value; OnPropertyChanged(); } }
        private string _Query;
        public string Query { get => _Query; set { _Query = value; OnPropertyChanged(); } }
        bool clicked = false;
        public ICommand AddCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand PrintWindowCommand { get; set; }


        public WithdrawViewModel()
        {
            FilterList = new String[] { "Tên khách hàng", "Mã sổ tiết kiệm", "Loại tiết kiệm" };
            Init = new ObservableCollection<PHIEURUTTIEN>(DataProvider.Ins.DB.PHIEURUTTIENs);
            List = Init;
            ListSTK = new ObservableCollection<SOTIETKIEM>(DataProvider.Ins.DB.SOTIETKIEMs);

            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RutTienWindow addWithDraw = new RutTienWindow(this);
                addWithDraw.ShowDialog();
            });
            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ResetField();
                MaSo = "";
                p.Close();
            });
            SaveCommand = new RelayCommand<Window>(
                (p) =>
                {
                    if ((String.IsNullOrEmpty(MaSo)) != true && clicked==true )
                        return true;
                    else
                        return false;
                },
                (p) =>
                {
                    AddDeposit();
                    ResetField();
                });
            SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                List = Init;
                if (SelectedFilter == null || String.IsNullOrEmpty(Query))
                    List = Init;
                else
                {
                    switch (SelectedFilter)
                    {
                        case "Tên khách hàng":
                            List = new ObservableCollection<PHIEURUTTIEN>(List.Where(x => x.BiXoa != true && x.SOTIETKIEM.KHACHHANG.TenKhachHang.Contains(Query)));
                            break;
                        case "Mã sổ tiết kiệm":
                            List = new ObservableCollection<PHIEURUTTIEN>(List.Where(x => x.BiXoa != true && x.MaSo.ToString().Contains(Query)));
                            break;
                        case "Loại tiết kiệm":
                            List = new ObservableCollection<PHIEURUTTIEN>(List.Where(x => x.BiXoa != true && x.SOTIETKIEM.LOAITIETKIEM.TenLoaiTietKiem.ToString().Contains(Query)));
                            break;
                    }
                }
            });
            PrintCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                System.Windows.Controls.PrintDialog print = new System.Windows.Controls.PrintDialog();
                print.PrintVisual(p as Grid, "Phiếu rút tiền");
            });
            PrintWindowCommand = new RelayCommand<object>((p) => { return (SelectedItem != null); }, (p) =>
            {
                WithdrawVoucherPrint d = new WithdrawVoucherPrint(this);
                d.ShowDialog();
            });
            
        }
        private void ResetField()
        {
     
            CMND = "";
            TenKhachHang = "";
            SoTienRut = "";
            LoaiSTK = "";
            SoTienGui = "";
        }
        private void CheckMaSTK(string maSTK)
        {
            bool isIntMaSo = int.TryParse(maSTK, out int mastk);

            var stk = ListSTK.Where(x => x.MaSo == mastk && x.BiXoa != true).SingleOrDefault();
            if (stk == null)
            {
                clicked = false;
                ResetField();
            }
            else
            {
                CMND = stk.KHACHHANG.CMND;
                TenKhachHang = stk.KHACHHANG.TenKhachHang;
                LoaiSTK = stk.LOAITIETKIEM.TenLoaiTietKiem.ToString();
                SoTienGui = "Số dư khả dụng: " + TinhSoDu(maSTK) + " VNĐ";
                TimeSpan timedate = (TimeSpan)(DateTime.Now - stk.NgayMoSo);
                if (stk.LOAITIETKIEM.TenLoaiTietKiem == "Không kì hạn")
                {
                    if ((DateTime.Now - (DateTime)stk.NgayMoSo).TotalDays < stk.LOAITIETKIEM.ThoiGianGoiToiThieu)
                        MessageBox.Show("Chưa đủ 15 ngày để rút tiền. Số ngày mới gửi được: " + timedate.Days + " ngày");

                    else
                        MessageBox.Show(DateTime.Now + "---" + stk.NgayMoSo + "= " + timedate.Days.ToString());
                }
                else
                {
                    SoTienRut = TinhSoDu(maSTK).ToString();

                }


            }              
        }
        private bool checkTienRut(string maSTK)
        {
            bool isIntMaSo = int.TryParse(maSTK, out int mastk);
            bool isIntSoTienRut = int.TryParse(SoTienRut, out int soTienRut);
            if (soTienRut == 0)
            {
                clicked = false;
                return false;
            }
            var stk = ListSTK.Where(x => x.MaSo == mastk && x.BiXoa != true).SingleOrDefault();
            if (stk == null)
            {
                clicked = false;
                return false;
            }
            else
            {
                if(stk.LOAITIETKIEM.TenLoaiTietKiem=="Không kì hạn")
                {
                    if (soTienRut <= TinhSoDu(stk.MaSo.ToString()))
                        {
                        clicked = true;
                        return true;
                    }
                    else {
                        clicked = false;
                        return false; 
                    }
                }
                else
                {
                    if (soTienRut!=TinhSoDu(mastk.ToString()))
                    {
                        SoTienRut = TinhSoDu(maSTK).ToString();

                         clicked = true;
                          return true;
                    }
                    else
                    {
                        clicked = true;
                        return true;
                    }
                }
            }
            return false;

        }
        private bool kiemTraDateKhongKyHan(DateTime ngayMoSo)
        {
            TimeSpan timedate = (TimeSpan)(DateTime.Now - ngayMoSo);

            if (timedate.Days > 15) return true;
            return false;
        }
        private double TinhSoDu(string maSTK)
        {
            long soDu = 0;
            bool isIntMaSo = int.TryParse(maSTK, out int _mastk);
            bool isIntSoTienRut = int.TryParse(SoTienRut, out int soTienGoi);
            if (!isIntMaSo ) return 0;

            var stk = ListSTK.Where(x => x.MaSo == _mastk && x.BiXoa != true).SingleOrDefault();
            if (stk.LOAITIETKIEM.TenLoaiTietKiem == "Không kì hạn")
            {
                if ((DateTime.Now - (DateTime)stk.NgayMoSo).TotalDays < stk.LOAITIETKIEM.ThoiGianGoiToiThieu) return 0;
                else
                soDu= (long)(stk.SoTienGoi+((int)((decimal)stk.SoTienGoi * (decimal)stk.LOAITIETKIEM.LaiSuat * (int)(DateTime.Now - (DateTime)stk.NgayTinhLaiGanNhat).TotalDays / 36000)));
              //  MessageBox.Show(((decimal)stk.SoTienGoi * (decimal)stk.LOAITIETKIEM.LaiSuat * (decimal)(DateTime.Now - (DateTime)stk.NgayTinhLaiGanNhat).TotalDays / 36000).ToString());
                return soDu;
            }

            else
            {
                if ((DateTime.Now - (DateTime)stk.NgayMoSo).TotalDays < stk.LOAITIETKIEM.ThoiGianGoiToiThieu)
                {
                    MessageBox.Show("Chưa đến kì hạn tính lãi suất để có thể rút tiền!");
                    clicked = false;
                    return 0;
                }
                var koKiHan = DataProvider.Ins.DB.LOAITIETKIEMs.Where(x => x.TenLoaiTietKiem == "Không kì hạn").SingleOrDefault();
                if (koKiHan == null) return 0;
                var laiSuatKoKiHan = koKiHan.LaiSuat;
                int laiKiHan;
                int laiKhongKiHan;
                if (((DateTime)stk.NgayTinhLaiGanNhat - (DateTime)stk.NgayMoSo).TotalDays < stk.LOAITIETKIEM.ThoiGianGoiToiThieu)
                {
                 //   MessageBox.Show("cc");
                    laiKiHan = (int)((decimal)stk.SoTienGoi * (decimal)stk.LOAITIETKIEM.LaiSuat * stk.LOAITIETKIEM.ThoiGianGoiToiThieu / 36000);
                    laiKhongKiHan = (int)((decimal)stk.SoTienGoi * (decimal)laiSuatKoKiHan * (int)((DateTime.Now - (DateTime)stk.NgayTinhLaiGanNhat).TotalDays - stk.LOAITIETKIEM.ThoiGianGoiToiThieu) / 36000);
                }
                else
                {
                    laiKiHan = 0;
                    laiKhongKiHan = (int)((decimal)stk.SoTienGoi * (decimal)laiSuatKoKiHan * (int)(DateTime.Now - (DateTime)stk.NgayTinhLaiGanNhat).TotalDays / 36000);
                }
                soDu = (int)(stk.SoTienGoi+ laiKiHan + laiKhongKiHan);
                return soDu;
            }

        }
        private void AddDeposit()
        {

            bool isIntMaSo = int.TryParse(MaSo, out int maSo);
            bool isIntSoTienRut = int.TryParse(SoTienRut, out int soTienRut);
            if (!isIntMaSo || !isIntSoTienRut) return;

            var stk = DataProvider.Ins.DB.SOTIETKIEMs.Where(x => x.MaSo == maSo && x.BiXoa !=true).SingleOrDefault();
            if (stk == null) return;

            CheckMaSTK(MaSo);
            if (checkTienRut(MaSo))
            {
               // MessageBox.Show("Vô nút save"+ "sotiengoi: "+stk.SoTienGoi);
                stk.SoTienGoi = (int)(TinhSoDu(MaSo) - soTienRut);
               // MessageBox.Show(stk.SoTienGoi.ToString()+"= sodu:"+TinhSoDu(MaSo)+"+ sotienrut: "+soTienRut);
                stk.NgayTinhLaiGanNhat = DateTime.Now;
                if (stk.SoTienGoi == 0)
                {
                    stk.BiXoa = true;
                }
                var result2 = MessageBox.Show("Bạn muốn rút số tiền: "+SoTienRut.ToString(),
                "Kiểm tra lại thao tác",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                if (result2.ToString() == "Yes")
                {
                    var PHIEURUTTIEN = new PHIEURUTTIEN { SOTIETKIEM = stk, SoTienRut = soTienRut, MaSo = maSo, NgayRut = DateTime.Now };
                    DataProvider.Ins.DB.PHIEURUTTIENs.Add(PHIEURUTTIEN);
                    DataProvider.Ins.DB.SaveChanges();
                    List.Add(PHIEURUTTIEN);
                    MaSo = "";
                }
                else {

                    MaSo = "";
                }
               
            }

        }
    }
}
