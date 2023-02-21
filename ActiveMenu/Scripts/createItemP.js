$("#languagetOption").change(function () {
    let id = $(this).val()
    if (id != '') {

        window.location.href = "/Home/ChangeLanguage?lang=" + id + "&actionstring=createItem";

    }
});
$("#addnew").click(function () {
    window.location.href = "/Home/createItem"
})
$(".default").click(function () {
    $('#imageFileHolder').trigger('click');
})


//
$("#imageFileHolder").on('change', function () {

    var input = this;
    var url = $(this).val()
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    var filename = url.substring(url.lastIndexOf('\\') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {

        size = this.files[0].size;
        filename = this.files[0].filename;
        var processID = makeid();
        var htmldiv = `<div  class="imageCover"> <img id="` + processID + `" src="#" /> </div>`;

        if (size > 500000) {
            toastr.options = {
                "debug": false,
                "positionClass": "toast-top-center",
                "onclick": null,
                "fadeIn": 300,
                "fadeOut": 1000,
                "timeOut": 5000,
                "extendedTimeOut": 1000
            }
            toastr.error(' image size must be under 500 kb')
        }
        else {
            var reader = new FileReader();
            reader.addEventListener("load", function (e) {


                img = new Image();
                img.src = e.target.result;
                img.onload = function () {

                    $("#imageHolder").append(htmldiv);
                    var mydiv = document.getElementById(processID);
                    mydiv.src = reader.result;

                    sendFile(key + ";;;" + processID, input.files[0]);
                  

                    // alert("Width:" + this.width + "   Height: " + this.height);//this will give you image width and height and you can easily validate here....
                };



                //$("#imageHolder").append(htmldiv);
                //var mydiv = document.getElementById(processID);
                //mydiv.src = reader.result;

            }, false);

            reader.readAsDataURL(input.files[0]);
        }
       

        let key = $("#fileupload").val();

       


        //sendFile(key + ";;;" + processID, input.files[0]);

    }



    //var url = $(this).val()
    //var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    //var filename = url.substring(url.lastIndexOf('\\') + 1).toLowerCase();
    //if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {

    //    size = this.files[0].size;
    //    filename = this.files[0].filename;

    //    if (size > 10000) {

    //        toastr.options = {
    //            "debug": false,
    //            "positionClass": "toast-top-center",
    //            "onclick": null,
    //            "fadeIn": 300,
    //            "fadeOut": 1000,
    //            "timeOut": 5000,
    //            "extendedTimeOut": 1000
    //        }
    //        toastr.error('  بیش از 1 مگا بایت است ' + file.name + ' سایز عکس')
    //    }






    //    var processID = makeid();
    //    var htmldiv = `<div  class="imageCover"> <img id="` + processID + `" src="#" /> </div>`;



    //    let key = $("#fileupload").val();

    //    var reader = new FileReader();
    //    reader.addEventListener("load", function () {

    //        img = new Image();
    //        img.onload = function () {
    //            alert("width: " + this.width)
    //            alert("height: " + this.height)
    //            if (this.width < this.height) {
    //                toastr.options = {
    //                    "debug": false,
    //                    "positionClass": "toast-top-center",
    //                    "onclick": null,
    //                    "fadeIn": 300,
    //                    "fadeOut": 1000,
    //                    "timeOut": 5000,
    //                    "extendedTimeOut": 1000
    //                }
    //                toastr.error(' عکس باید به صورت پرتره و افقی باشدی')
    //            }
    //            else {
    //                $("#imageHolder").append(htmldiv);
    //                var mydiv = document.getElementById(processID);
    //                mydiv.src = reader.result;
    //                sendFile(key + ";;;" + processID, input.files[0]);
    //            }

    //            // alert("Width:" + this.width + "   Height: " + this.height);//this will give you image width and height and you can easily validate here....
    //        };


    //    }, false);

    //    reader.readAsDataURL(input.files[0]);




    //}


});
var makeid = function () {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < 10; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
};
var sendFile = function (key, value) {

    let progressID = key.split(';;;')[1];
    key = key.split(';;;')[0];

    var ext = key.substring(key.lastIndexOf('.') + 1).toLowerCase();
    var filename = key.substring(key.lastIndexOf('\\') + 1).toLowerCase();
    let messageType = null;
    if (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg") {
        messageType = 'image';
    }
    else {
        messageType = 'file';
    }


    var formData = new FormData();
    var fileName = key
    formData.append('blob', value);
    formData.append('filename', fileName);
    let request = new XMLHttpRequest();
    request.open('POST', '/admin/sendToServer');
    request.upload.addEventListener('progress', function (e) {

        let percent_complete = parseInt((e.loaded / e.total) * 100);

        let classname = '';
        if (percent_complete >= 50) {
            classname = 'over50';
        }




        let srt = `<div  class=" progress-circlle ` + classname + `  p` + percent_complete + ` "><span>` + percent_complete + `%</span><div class="left-half-clipper"><div class="first50-bar"></div><div class="value-bar"></div></div></div>`;
        var spg = $("#" + progressID).parent();

        spg.children('.progress-circlle').each(function () {
            //this.remove();
        })
        spg.append(srt);

        //console.log(e.loaded / e.total + "-" + percent_complete + "%");
    });

    // AJAX request finished event
    request.addEventListener('load', function (e) {

        var ext = key.substring(key.lastIndexOf('.') + 1).toLowerCase();
        var filename = key.substring(key.lastIndexOf('\\') + 1).toLowerCase();


        var ul = $(".messages ul");

        let rsp = request.response;

        var spg = $("#" + progressID).parent();
        var name = "" + rsp;


        spg.children('.progress-circlle').each(function () {
            this.remove();
        })
        let check = `<img  onclick = removeImage("` + progressID + `***` + name + `")   id="R` + progressID + `" class="removeImage" src="/images/app/trash.png" style="position:absolute; bottom:0;top:0;left:0;right:0;margin:auto; width:30px" /> <i class="fa fa-remove" style="font-size: 12px;position: absolute;bottom: 5px;left: 0px;color: white;"></i>`;
        spg.append(check);
        //onclick=removeImage(` + rsp + `)
    })
    // send POST request to server side script
    request.send(formData);

};
$(".removeImage").click(function () {
    var element = $(this)
    var name = element.attr('id');
    $.ajax({
        url: "/admin/removeimage",
        data: {
            id: name
        },
        success: function () {
            element.parent().remove();
        }
    })
    //            $.ajax({
    //                url: "/home/removeimage",
    //                data: {
    //                    id: name
    //                },
    //                success: function () {
    //                    alert("#P" + name);
    //                    //$("#P" + name).remove();
    //.                }
    //            })
});
var removeImage = function (DAT) {
    let id = "R" + DAT.split('***')[0];
    let name = DAT.split('***')[1];
    $.ajax({
        url: "/admin/removeimage",
        data: {
            id: name
        },
        success: function () {
            $("#" + id).parent().remove();
        }
    })
}
//

$(document).ready(function () {
    $(".fa-file").addClass("act")
})


