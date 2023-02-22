
# Đọc kỹ để đỡ đau khổ ❤




## Code "Xanh sạch đẹp"

 - [Coding Convention](https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md)
 - [Format Code: Ctrl + K + D](https://media.makeameme.org/created/bad-code-bad.jpg)
## Trước khi tạo merge request thì làm ơn 😭

- LÀM ƠN CHANGES FILE ÍT THÔI => Tội Reviewer lắm 😢
- Pull code từ master về => "CHECK THẬT KỸ"
=> Nếu phát hiện ra lỗi báo ngay cho mọi người và BẮT người vừa COMMIT "SỬA NHIỆT TÌNH" nhé 😉
- Tiến hành merge từ "master" về "local" => lại TEST "THẬT KỸ" (nếu muốn đơn giản thì hãy viết test cho phần mình làm nhé 😉)
- Tiến hành tạo Merge Request => Sau khi branch mình đã được merge thì "Xin vui lòng hét lớn thông báo là t vừa được merge xong để mn còn biết"
- LƯU Ý: mn đổi pass SQL => user: sa; pass: 123 => mục đích là cho đỡ phải chỉnh sửa

## Support 1

Cài Tools

```bash
  dotnet tool install --global dotnet-ef
```

Migration

```bash
  dotnet ef migrations add NewMigration -s APIs -p Infrastructures
```

UpdateDB

```bash
  dotnet ef database update -s APIs -p Infrastructures
```




## Git víp pro

Tại nhánh của mình

```bash
  git pull origin master
```


```bash
  git add .
```
```bash
  git commit -m"Nội dung commit"
```
```bash
  git push origin "Tên Nhánh"
```

Lên gitlab tạo merge request.



## Thắc mắc

- Database chuẩn => Mọi thứ kết nối được với nhau => Lấy được hết thông tin liên quan giữa các Model với nhau.Nếu cảm thấy không làm được vui lòng "[bấm vào](https://letmegooglethat.com/)", rồi hãy đi hỏi bạn "bè" nhé 😉.
- Mọi người code thế nào cũng được đi đường vòng hay ngắn không quan trọng. Đầu tiên hãy làm thỏa mãn yêu cầu trước đã rồi hãy optimize sau.


##

![Logo](https://www.memecreator.org/static/images/memes/4023011.jpg)

