# MailClientApp

MailClientApp là ứng dụng WinForms giúp bạn đăng nhập và đọc email từ các nhà cung cấp phổ biến như Gmail, Yahoo, Outlook (Outlook.com, Hotmail...) thông qua IMAP.  
Ứng dụng hỗ trợ xác thực truyền thống (mật khẩu ứng dụng) và xác thực hiện đại OAuth2 cho Outlook.

## Tính năng

- Đăng nhập Gmail/Yahoo bằng mật khẩu ứng dụng (App Password).
- Đăng nhập Outlook bằng OAuth2 (IMAP XOAUTH2, không cần nhập mật khẩu).
- Đọc, liệt kê email từ hộp thư đến (Inbox).
- Đánh dấu email đã đọc.
- Giao diện WinForms đơn giản, dễ sử dụng.

## Yêu cầu

- .NET Framework 4.8
- Visual Studio 2022 trở lên
- Các thư viện NuGet:
  - [MailKit](https://www.nuget.org/packages/MailKit)
  - [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client)

## Cài đặt

1. **Clone dự án về máy:**
2. **Mở giải pháp bằng Visual Studio.**

3. **Khôi phục các gói NuGet:**
   - Chuột phải vào solution > Restore NuGet Packages.

4. **Cấu hình Azure App (bắt buộc cho Outlook OAuth2):**
   - Đăng ký ứng dụng tại [Azure Portal - App registrations](https://portal.azure.com/#blade/Microsoft_AAD_RegisteredApps/ApplicationsListBlade).
   - Chọn loại tài khoản: `Accounts in any organizational directory and personal Microsoft accounts`.
   - Thêm Redirect URI: `http://localhost`
   - Lấy `ClientId` và thay vào biến `ClientId` trong file `LoginForm.cs`.
   - Không cần cấp quyền IMAP cho tài khoản cá nhân (Outlook.com, Hotmail...).

5. **Build và chạy ứng dụng.**

## Hướng dẫn sử dụng

### Đăng nhập Gmail/Yahoo

1. Chọn nhà cung cấp là **Gmail** hoặc **Yahoo**.
2. Nhập địa chỉ email và **mật khẩu ứng dụng** (App Password).
3. Nhấn **Login**.

> Lưu ý: Gmail/Yahoo yêu cầu bật xác thực 2 bước và tạo App Password.

### Đăng nhập Outlook (Outlook.com, Hotmail...)

1. Chọn nhà cung cấp là **Outlook**.
2. Nhập địa chỉ email.
3. Nhấn **Đăng nhập Microsoft**.
4. Đăng nhập và cấp quyền cho ứng dụng trên cửa sổ Microsoft hiện ra.
5. Sau khi xác thực thành công, ứng dụng sẽ tự động lấy mail từ hộp thư đến.

## Debug & Hỗ trợ

- Nếu không load được mail, ứng dụng sẽ hiển thị danh sách mailbox thực tế để bạn kiểm tra.
- Nếu gặp lỗi xác thực, kiểm tra lại access token, scope, hoặc cấu hình Azure App.
- Nếu cần hỗ trợ, hãy tạo issue trên GitHub hoặc liên hệ tác giả.

## Đóng góp

Mọi đóng góp, pull request đều được hoan nghênh!

## License

MIT License
