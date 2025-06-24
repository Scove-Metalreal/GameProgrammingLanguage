using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Lab06;

class Program
{
    private const string FIREBASE_DB_URL = "https://lab06-361d2-default-rtdb.asia-southeast1.firebasedatabase.app/";
    
    static async Task Main(string[] args)
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential =
                    GoogleCredential.FromFile("D:\\Scove Folder\\GameProgrammingLanguage\\LAB\\Lab06\\lab06.json")
            });

            bool tiepTuc = true;

            while (tiepTuc)
            {
                Console.Clear();
                Console.WriteLine("=========== QUAN LY SINH VIEN ==========");
                Console.WriteLine("1. Them sinh vien");
                Console.WriteLine("2. Tim sinh vien");
                Console.WriteLine("3. Cap nhap sinh vien");
                Console.WriteLine("4. Xoa sinh vien");
                Console.WriteLine("5. Sap xep danh sach sinh vien");
                Console.WriteLine("0. Thoat");
                Console.WriteLine("=========================================");
                Console.Write("Lua chon: ");

                string input = Console.ReadLine()?.Trim().ToLower();

                switch (input)
                {
                    case "1":
                        await ThemSinhVien();
                        break;
                    case "2":
                        await TimSinhVien();
                        break;
                    case "3":
                        await CapNhapSinhVien();
                        break;
                    case "4":
                        await XoaSinhVien();
                        break;
                    case "5":
                        await SapXepDanhSachSinhVien();
                        break;
                    case "0" :
                    case "exit":
                        tiepTuc = false;
                        Console.WriteLine("\nGoodbye!");
                        break;
                    default:
                        Console.WriteLine("Loi tuy chon. Vui long thu lai.");
                        break;
                }
                if (tiepTuc)
                {
                    Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private static async Task ThemSinhVien()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        Console.Write("Nhap ma sinh vien: ");
        int mssv = int.Parse(Console.ReadLine());
        
        Console.Write("Nhap ten sinh vien: ");
        string hoTen = Console.ReadLine();
        
        Console.Write("Nhap diem: ");
        int diem = int.Parse(Console.ReadLine());
        
        Console.Write("Nhap lop: ");
        string lop = Console.ReadLine();    
        
        SinhVien sv = new SinhVien(mssv, hoTen, diem, lop);

        await firebase.Child("DanhSachSinhVien").Child(sv.MSSV.ToString()).PutAsync(sv);

        Console.WriteLine("Them sinh vien thanh cong");
    }

    private static async Task TimSinhVien()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        Console.Write("Nhap ma sinh vien: ");
        string mssv = Console.ReadLine();

        try
        {
            var sinhVien = await firebase
                .Child("DanhSachSinhVien")
                .Child(mssv)
                .OnceSingleAsync<SinhVien>();

            if (sinhVien == null || string.IsNullOrEmpty(sinhVien.HoTen))
            {
                Console.WriteLine("Khong tim thay sinh vien voi ID: " + mssv);
                return;
            }

            Console.WriteLine("MSSV: " + sinhVien.MSSV);
            Console.WriteLine("Ho ten: " + sinhVien.HoTen);
            Console.WriteLine("Diem: " + sinhVien.Diem);
            Console.WriteLine("Lop: " + sinhVien.Lop);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Loi doc danh sach sinh vien: {e.Message}");
        }
    }

    private static async Task CapNhapSinhVien()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        Console.Write("Nhap ma sinh vien: ");
        string mssv = Console.ReadLine();

        try
        {
            var sinhVienHienTai = await firebase
                .Child("DanhSachSinhVien")
                .Child(mssv)
                .OnceSingleAsync<SinhVien>();

            if (sinhVienHienTai == null || string.IsNullOrEmpty(sinhVienHienTai.HoTen))
            {
                Console.WriteLine("Khong tim thay sinh vien voi ID: " + mssv);
                return;
            }

            Console.WriteLine($"Ten Hien tai: {sinhVienHienTai.HoTen}");
            Console.WriteLine($"Diem hien tai: {sinhVienHienTai.Diem}");
            Console.WriteLine($"Lop hien tai: {sinhVienHienTai.Lop}");

            Console.WriteLine("\nNhap du lieu moi (de trong de bo qua):");

            Console.WriteLine("Nhap ten moi: ");
            string tenMoi = Console.ReadLine();
            if (!string.IsNullOrEmpty(tenMoi))
                sinhVienHienTai.HoTen = tenMoi;

            Console.WriteLine("Nhap diem moi: ");
            string diemMoi = Console.ReadLine();
            if (!string.IsNullOrEmpty(diemMoi))
                sinhVienHienTai.Diem = Int32.Parse(diemMoi);

            Console.WriteLine("Nhap lop moi:");
            string lopMoi = Console.ReadLine();
            if (!string.IsNullOrEmpty(lopMoi))
                sinhVienHienTai.Lop = lopMoi;

            
            await firebase.Child("DanhSachSinhVien").Child(sinhVienHienTai.MSSV.ToString()).PutAsync(sinhVienHienTai);

            Console.WriteLine("Cap nhap sinh vien thanh cong");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Loi doc danh sach sinh vien: {e.Message}");
        }
    }

    private static async Task XoaSinhVien()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        Console.Write("Nhap ma sinh vien: ");
        string mssv = Console.ReadLine();

        try
        {
            var sinhVienHienTai = await firebase
                .Child("DanhSachSinhVien")
                .Child(mssv)
                .OnceSingleAsync<SinhVien>();

            if (sinhVienHienTai == null || string.IsNullOrEmpty(sinhVienHienTai.HoTen))
            {
                Console.WriteLine("Khong tim thay sinh vien voi ID: " + mssv);
                return;
            }

            Console.WriteLine("Ban co muon xoa sinh vien nay khong? (y/n)");
            Console.WriteLine(sinhVienHienTai);
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                await firebase
                    .Child("DanhSachSinhVien")
                    .Child(mssv)
                    .DeleteAsync();

                Console.WriteLine($"Sinh vien voi ID {mssv} da duoc xoa thanh cong.");
            }
            else
            {
                Console.WriteLine("Da huy xoa sinh vien.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Loi doc danh sach sinh vien: {e.Message}");
        }
    }

    private static async Task SapXepDanhSachSinhVien()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        var myStudents = await firebase.Child("DanhSachSinhVien").OnceAsync<SinhVien>();

        var allStudents = myStudents.Select(s => s.Object).ToList();

        var sortedStudentsList = allStudents
            .OrderByDescending(s => s.Diem)
            .Take(5)
            .ToList();

        try
        {
            await firebase.Child("TopSinhVien")
                .PutAsync(sortedStudentsList);

            Console.WriteLine("Luu danh sach top sinh vien thanh cong.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Loi cap nhap danh sach sinh vien: {e.Message}");
        }
    }
}