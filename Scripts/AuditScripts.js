/// <reference path="../js/jquery-1.11.3.min.js" />
 
function FilterContant(obj) {
    
    var str = $(obj).val();
    var charCode = str.replace(/[^a-zA-Z ]/g, "");
    $(obj).val(charCode);

}

function FilterTextKey(e) {

    var charCode = (e.which) ? e.which : e.keyCode;

    var Data = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789- ";
    
    if (Data.indexOf(String.fromCharCode(charCode)) == -1 && charCode != 8 && charCode != 9) {
        return false;
    }
    else {
        return true;
    }
}

function addEvent() {
    
    $(".alphaN").on("keypress", function (e) {

        var charCode = (e.which) ? e.which : e.keyCode;

        var Data = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789- ";
        
        if (Data.indexOf(String.fromCharCode(charCode)) == -1 && charCode != 8 && charCode != 9) {
            return false;
        }
        else {
            return true;
        }

    });

    $(".alphaN").on("change", function () {

        var str = $(this).val();
        var charCode = str.replace(/[^a-zA-Z0-9\s-]/g, "");
        $(this).val(charCode);

    });


    $(".alpha").on("keypress", function (e) {

        var charCode = (e.which) ? e.which : e.keyCode;

        var Data = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        
        if (Data.indexOf(String.fromCharCode(charCode)) == -1 && charCode != 8 && charCode != 9) {
            return false;
        }
        else {
            return true;
        }

    });

    $(".alpha").on("change", function () {

        var str = $(this).val();
        var charCode = str.replace(/[^a-zA-Z\s]/g, "");
        $(this).val(charCode);

    });
} 

