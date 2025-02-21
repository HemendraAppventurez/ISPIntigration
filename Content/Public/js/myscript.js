$(window).scroll(function() {
var height = $(window).scrollTop();
if(height  > 150) {
$(".top-nav").addClass("pwdFxd");
}
if(height  < 150) {
$(".top-nav").removeClass("pwdFxd");
} 
});



$(document).ready(function(){
$('.leftbar').height($('.rightbar').height()+80);
//alert($('.leftbar').height())
//alert($('.rightbar').height())
$( ".sopen a").click(function() {
$( ".searchdiv").fadeToggle("slow");
});	
$('table').wrap('<div class="table-responsive">');	
$("table").addClass("table table-bordered table-striped")
$(".removetblclass table").removeClass("table table-bordered table-striped")	
$(".sitemap ul").removeClass("nav navbar-nav")
$(".sitemap li").removeClass("dropdown")
$(".sitemap li").removeClass("dropdown-submenu")
$(".sitemap li a").removeClass("dropdown-toggle")
$(".sitemap ul ul").removeClass("dropdown-menu")
$(".sitemap ul").removeAttr("id","menu")
$(".right-section-menu ul").removeClass("nav navbar-nav")
$(".right-section-menu ul").addClass("righ-menu")
$(".right-section-menu li").removeClass("dropdown")
$(".right-section-menu li a").removeClass("dropdown-toggle")
$(".right-section-menu ul ul").removeClass("dropdown-menu")
if($.cookie("css")) {
$("#MSS").attr("href",$.cookie("css"));
}
$(".defTheme").click(function() { 
$("#MSS").attr("href",$(this).attr('href'));
$.cookie("css",$(this).attr('href'));
return false;
});	
$(".hi-btn").click(function () {
        var cfrm = confirm("आपको चिकित्सा स्वास्थ्य एवं परिवार कल्याण विभाग, उत्तर प्रदेश की वेबसाइट के हिंदी संस्करण पर हस्तानांतरित किया जा रहा है ");
        if (cfrm == true) {
            window.location(this.window.url + "/hi");
            return true;
        }
        else if (cfrm == false) {
            return false;
        }
        //alert(crm);
    });

    var comp = new RegExp(location.host);
    $('a').each(function () {
        if (comp.test($(this).attr('href'))) {
            // a link that contains the current host 
            $(this).addClass('local');
        }
        else {
            // a link that does not contain the current host
            $(this).addClass('external');
        }
    });

    $('aa').filter(function () {
        return this.hostname && this.hostname !== location.hostname;
    })
    .click(function () {
        $(this).attr('target', '_blank');
        var x = window.confirm('You are about to leave the website. and view the content of an external website.');
        var val = false;
        if (x)
            val = true;
        else
            val = false;
        return val;
    });

 $(".skiper").click(function () {
                        $('html,body').animate({
                            scrollTop: 270
                        },
                            'slow');
                    });

                    var mk = $(location).attr('href')
                    $('.skiper').attr('href', mk + "#main-content");

});

//// Date Time Function
	
tday=new Array("Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday");
tmonth=new Array("January","February","March","April","May","June","July","August","September","October","November","December");

function GetClock(){
var d=new Date();
var nday=d.getDay(),nmonth=d.getMonth(),ndate=d.getDate(),nyear=d.getYear();
if(nyear<1000) nyear+=1900;
var nhour=d.getHours(),nmin=d.getMinutes(),nsec=d.getSeconds(),ap;

if(nhour==0){ap=" AM";nhour=12;}
else if(nhour<12){ap=" AM";}
else if(nhour==12){ap=" PM";}
else if(nhour>12){ap=" PM";nhour-=12;}

if(nmin<=9) nmin="0"+nmin;
if(nsec<=9) nsec="0"+nsec;

document.getElementById('datetime').innerHTML=""+tday[nday]+", "+tmonth[nmonth]+" "+ndate+", "+nyear+" "+nhour+":"+nmin+":"+nsec+ap+"";
}

window.onload=function(){
GetClock();
setInterval(GetClock,1000);
}

$(document).ready(function () {
$('.footer-secton .pra-policy ul li a, .footer-secton .ft-bottom a').each(function(){
   $(this).attr('title',$(this).text());  //or use $.trim($(this).text()) to remove white spaces.
});
});

/////////// END HERE


 