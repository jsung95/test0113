using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyProject.Model
{
    public class ProductShowModel : Notifier
    {

        public string Prod_code { get; set; }

        private string prod_name;
        public string Prod_name
        {
            get { return prod_name; }
            set
            {
                prod_name = value;                
                OnPropertyChanged("Prod_name");
            }
        }

        public string Category_name { get; set; }
        public int? Prod_price { get; set; }
        public int? Prod_total { get; set; }
        public int? Imp_dept_count { get; set; }

        private DateTime prod_expire;
        public DateTime Prod_expire
        {
            get { return prod_expire; }
            set
            {
                prod_expire = value;
                if(prod_expire < DateTime.Now)
                {
                    isExpired = true;
                }
                else
                {
                    isExpired = false;
                }
                OnPropertyChanged("Prod_expire");      
            }        
        }

        public int? Prod_id { get; set; }
        public int? Imp_dept_id { get; set; }
        public int? Prod_remainexpire { get; set; }




        //발주팝업박스에서 필요해서 임의로 만듬
        private int? volume;                       //용량
        public int? Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }

        private int? mount;                       //수량
        public int? Mount
        {
            get { return mount; }
            set
            {
                mount = value;
                OnPropertyChanged("Mount");
            }
        }

        private string manufacturer;            //제조사
        
        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                OnPropertyChanged(Manufacturer);
            }
        }

        private string orderMemo;                //메모
        public string OrderMemo
        {
            get { return orderMemo; }
            set
            {
                orderMemo = value;
                OnPropertyChanged(OrderMemo);
            }
        }






        // 재고 출고 - 출고 유형 콤보박스에 들어갈 리스트
        private string[] selectedOutTypeList = new[] { "사용", "이관", "폐기" };
        public string[] SelectedOutTypeList
        {
            get { return selectedOutTypeList; }
            set
            {
                selectedOutTypeList = value;
                OnPropertyChanged("SelectedOutTypeList");
            }
        }

        // 재고 출고 - 선택한 출고 유형 콤보박스를 담을 값
        private string selectedOutType;
        public string SelectedOutType
        {
            get { return selectedOutType; }
            set
            {
                selectedOutType = value;
                if (selectedOutType == null)
                {
                    IsEnabled = false;
                }

                Console.WriteLine("SelectedOutType 변경합니다! : " + selectedOutType);
                if (selectedOutType == "사용")
                {
                    Popup_combobox_vis = Visibility.Hidden;
                    IsEnabled = true;
                }
                else if (selectedOutType == "이관")
                {
                    Popup_combobox_vis = Visibility.Visible;                    
                    IsEnabled = true;
                }
                else if (selectedOutType == "폐기")
                {
                    Popup_combobox_vis = Visibility.Hidden;                    
                    IsEnabled = true;
                }

                OnPropertyChanged("SelectedOutType");
            }
        }
        // 재고 입고(팝업박스) - 입고 유형을 담을 값
        private string selectedInType = "추가";
        public string SelectedInType
        {
            get { return selectedInType; }
            set
            {
                selectedInType = value;
                OnPropertyChanged("SelectedInType");
            }
        }
        // 출고 팝업박스 - 부서 선택 콤보 박스의 Visibility와 바인딩
        private Visibility popup_combobox_vis;
        public Visibility Popup_combobox_vis
        {
            get { return popup_combobox_vis; }
            set
            {
                popup_combobox_vis = value;
                Console.WriteLine("popup_combobox_vis 변경합니다! : " + popup_combobox_vis);
                OnPropertyChanged("Popup_combobox_vis");
            }
        }
        
        // 재고 출고 - 선택한 출고(이관) 부서를 담을 프로퍼티
        private DeptModel selectedOutDept;
        public DeptModel SelectedOutDept
        {
            get { return selectedOutDept; }
            set
            {
                selectedOutDept = value;
                //Console.WriteLine("선택한 출고(이관) 부서명 : " + selectedOutDept.Dept_name);
                OnPropertyChanged("SelectedOutDept");
            }
        }

        // 재고 출고 - 입력한 출고 수량을 담을 프로퍼티
        private int? inputOutCount;
        public int? InputOutCount
        {
            get { return inputOutCount; }
            set
            {
                inputOutCount = value;
                OnPropertyChanged("InputOutCount");
            }
        }
        // 재고 입고 - 입력한 입고 수량을 담을 프로퍼티
        private int? inputInCount;
        public int? InputInCount
        {
            get { return inputInCount; }
            set
            {
                inputInCount = value;
                OnPropertyChanged("InputInCount");
            }
        }

        //재고 출고 팝업박스 - 확인 버튼 활성화/비활성화 프로퍼티
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        // 유통기한 만료 여부
        private bool isExpired;
        public bool IsExpired
        {
            get { return isExpired; }
            set
            {
                isExpired = value;
                if (isExpired)
                    datePicker_vis = Visibility.Visible;
                else
                    datePicker_vis = Visibility.Hidden;
                OnPropertyChanged("IsExpired");
            }
        }
        // 입고 팝업박스 datepicker
        private Visibility datePicker_vis = Visibility.Hidden;
        public Visibility DatePickerVis
        {
            get { return datePicker_vis; }
            set
            {
                datePicker_vis = value;
                Console.WriteLine("DatePickerVis : " + datePicker_vis);              
                OnPropertyChanged("DatePickerVis");
            }
        }


    }//class

}//namespace
