SysPacket = {}
////SysPacket.loginClick = function (form) {
////    $('logFrm').password = "123";
////    if (!$('logFrm').password())
////        form.password($('[type=password]').val());
////        $.ajax({
////            type: "POST",
////            url: "/Login/DoAction",
////            data: ko.toJSON(self.form),
////            dataType: "json",
////            contentType: "application/json",
////            success: function (d) {
////                if (d.status == 'success') {
////                    message("登陆成功正在跳转，请稍候...");
////                    window.location.href = 'Home/Index';
////                } else {
////                    message(d.message);
////                }
////            },
////            error: function (e) {
////                message(e.responseText);
////            },
////            beforeSend: function () {
////                $(form).find("input").attr("disabled", true);
////                message("正在登陆处理，请稍候...");
////            },
////            complete: function () {
////                $(form).find("input").attr("disabled", false);
////            }
////        });
////};