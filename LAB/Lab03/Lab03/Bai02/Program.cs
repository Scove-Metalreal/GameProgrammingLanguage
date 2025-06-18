using System.Collections;

namespace Bai02
{
    internal class Program
    {
        static void ThemMatHang(Hashtable ds, MatHang mh)
        {
            ds.Add(mh.MaMH, mh);
        }

        static bool TimMatHang(Hashtable ds, int mmh)
        {
            return ds.ContainsKey(mmh);
        }

        static void XoaMatHang(Hashtable ds, int mmh)
        {
            if (TimMatHang(ds, mmh))
            {
                ds.Remove(mmh);
            }
        }
        static void Main(string[] args)
        {
            Hashtable ds = new Hashtable();

            bool addMore = true;

            while (addMore)
            {
                Console.WriteLine("Nhap vao ma mat hang: ");
                int maMH = int.Parse(Console.ReadLine());

                Console.WriteLine("Nhap vao ten mat hang: ");
                string tenMH = Console.ReadLine();

                Console.WriteLine("Nhap vao so luong mat hang: ");
                int soLuong = int.Parse(Console.ReadLine());

                Console.WriteLine("Nhap vao don gia: ");
                float donGia = float.Parse(Console.ReadLine());

                MatHang mh = new MatHang(maMH, tenMH, soLuong, donGia);

                if (!TimMatHang(ds, mh.MaMH))
                {
                    ThemMatHang(ds, mh);
                } else
                {
                    Console.WriteLine("Mat hang da ton tai");
                }


                Console.WriteLine("Tiep tuc them: (Y/N)");
                string keepAdding = Console.ReadLine().ToUpper();

                if (keepAdding.Equals("Y"))
                {
                    addMore = true;
                } else if (keepAdding == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Cu phap ko dung. Breaking");
                    break;
                }
            }

            foreach(DictionaryEntry item in ds)
            {
                Console.WriteLine($"{ item.Key}: { item.Value}");
            }

            Console.WriteLine("Nhap vao ma mat hang: ");
            int searchID = int.Parse(Console.ReadLine());

            if (TimMatHang(ds, searchID))
            {
                Console.WriteLine("Mat hang ton tai");
             
                Console.WriteLine("Ban co muon xoa: ");
                string agree = Console.ReadLine().ToUpper();
                if (agree.Equals("Y"))
                {
                    XoaMatHang(ds, searchID);
                } 

                foreach (var item in ds)
                {
                    Console.WriteLine(item);
                }
            } else
            {
                Console.WriteLine("Mat hang khong ton tai");
            }

        }
    }
}
