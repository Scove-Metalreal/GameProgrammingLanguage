namespace Lab06;

public class SinhVien
{
    public int MSSV { get; set; }
    public string HoTen { get; set; }
    public int Diem { get; set; }
    public string Lop { get; set; }

    public SinhVien()
    {
        this.MSSV = 0;
        this.HoTen = null;
        this.Diem = 0;
        this.Lop = null;;
    }

    public SinhVien(int mssv, string hoTen, int diem, string lop)
    {
        MSSV = mssv;
        HoTen = hoTen;
        Diem = diem;
        Lop = lop;
    }

    public override string ToString()
    {
        return $"ID sinh vien: {MSSV} - Ho ten: {HoTen}\nDiem: {Diem}\nLop: {Lop}";
    }
}