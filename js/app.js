/** ********************************************** **
	@Author			Scartheme .
	@Last Update	April 9, 2017
*************************************************** **/

$(document).ready(function () {
    AppDatapp = {
        appinit: function () {
            AppDatapp.HandleSidebartoggle();
            AppDatapp.Handlelpanel();
            AppDatapp.Handlelpanelmenu();
            AppDatapp.Handlethemeoption();
            AppDatapp.Handlesidebareffect();
            AppDatapp.Handlesidebarposition();
            AppDatapp.Handlecontentheight();
            AppDatapp.Handlethemecolor();
            AppDatapp.Handlenavigationtype();
            AppDatapp.Handlesidebarside();
            AppDatapp.Handleactivestatemenu();
            AppDatapp.Handlethemelayout();
            AppDatapp.Handlethemebackground();


        },
        Handlethemebackground: function () {
            function setthemebgcolor() {
                $('#theme-color > a.theme-bg').on("click", function () {
                    $('body').attr("theme-bg", $(this).attr("app-themebg-type"));
                });
            };
            setthemebgcolor();
        },
        Handlethemelayout: function () {
            $('#theme-layout').on("change", function () {
                if ($(this).val() == 'box-layout') {
                    $('body').attr("theme-layout", "box-layout");
                } else {
                    $('body').attr("theme-layout", "wide-layout");
                }
            });
        },
        Handleactivestatemenu: function () {
            $(".panel-list li:not('.app-has-menu') > a").on("click", function () {
                if ($('body').attr("app-navigation-type") == "vertical" || $('body').attr("app-navigation-type") == "vertical-compact") {
                    if ($(this).closest('li.app-has-menu').length === 1) {
                        $(this).closest('.panel-list').find('li.active').removeClass('active');
                        $(this).parent().addClass('active');
                        $(this).parent().closest('.app-has-menu').addClass('active');
                        $(this).parent('li').closest('li').closest('.app-has-menu').addClass('active');
                    } else {
                        $(this).closest('.panel-list').find('li.active').removeClass('active');
                        $(this).closest('.panel-list').find('li.opened').removeClass('opened');
                        $(this).closest('.panel-list').find('ul:visible').slideUp('fast');
                        $(this).parent().addClass('active');

                    }
                }
            });
        },
        Handlesidebarside: function () {
            $('#navigation-side').on("change", function () {
                if ($(this).val() == 'rightside') {
                    $('body').attr("app-nav-placement", "right");
                    $('body').attr("app-navigation-type", "vertical");
                    $('#app-wrapper').removeClass("compact-hmenu");
                } else {
                    $('body').attr("app-nav-placement", "left");
                    $('body').attr("app-navigation-type", "vertical");
                    $('#app-wrapper').removeClass("compact-hmenu");
                }
            });
        },
        Handlenavigationtype: function () {
            $('#navigation-type').on("change", function () {
                if ($(this).val() == 'horizontal') {
                    $('body').attr("app-navigation-type", "horizontal");
                    $('#app-wrapper').removeClass("compact-hmenu");
                    $('#app-header, #app-container').removeClass("app-minimized-lpanel");
                    $('body').attr("app-nav-placement", "left");
                    $('#app-header').attr("app-color-type", "logo-bg7");

                } else if ($(this).val() == 'horizontal-compact') {
                    $('body').attr("app-navigation-type", "horizontal");
                    $('#app-wrapper').addClass("compact-hmenu");
                    $('#app-header, #app-container').removeClass("app-minimized-lpanel");
                    $('body').attr("app-nav-placement", "left");
                    $('#app-header').attr("app-color-type", "logo-bg7");
                } else if ($(this).val() == 'vertical-compact') {
                    $('body').attr("app-navigation-type", "vertical-compact");
                    $('#app-wrapper').removeClass("compact-hmenu");
                    $('#app-header, #app-container').addClass("app-minimized-lpanel");
                    $('body').attr("app-nav-placement", "left");
                } else {
                    $('body').attr("app-navigation-type", "vertical");
                    $('#app-wrapper').removeClass("compact-hmenu");
                    $('#app-header, #app-container').removeClass("app-minimized-lpanel");
                    $('body').attr("app-nav-placement", "left");
                }
            });
        },

        Handlethemecolor: function () {

            function setheadercolor() {
                $('#theme-color > a.header-bg').on("click", function () {
                    $('#app-header > .app-right-header').attr("app-color-type", $(this).attr("app-color-type"));
                });
            };

            function setlpanelcolor() {
                $('#theme-color > a.lpanel-bg').on("click", function () {
                    $('#app-container').attr("app-color-type", $(this).attr("app-color-type"));
                });
            };

            function setllogocolor() {
                $('#theme-color > a.logo-bg').on("click", function () {
                    $('#app-header').attr("app-color-type", $(this).attr("app-color-type"));
                });
            };
            setheadercolor();
            setlpanelcolor();
            setllogocolor();
        },
        Handlecontentheight: function () {

            function setHeight() {
                var WH = $(window).height();
                var HH = $("#app-header").innerHeight();
                var FH = $("#footer").innerHeight();
                var contentH = WH - HH - FH - 2;
                var lpanelH = WH - HH - 2;
                $("#main-content ").css('min-height', contentH)
                $(".inner-left-panel ").css('height', lpanelH)

            };
            setHeight();

            $(window).resize(function () {
                setHeight();
            });
        },
        Handlesidebarposition: function () {

            $('#sidebar-position').on("change", function () {
                if ($(this).val() == 'fixed') {
                    $('#app-left-panel,.app-left-header').attr("app-position-type", "fixed");
                } else {
                    $('#app-left-panel,.app-left-header').attr("app-position-type", "absolute");
                }
            });
        },
        Handlesidebareffect: function () {
            $('#leftpanel-effect').on("change", function () {
                if ($(this).val() == 'overlay') {
                    $('#app-header, #app-container').attr("app-lpanel-effect", "overlay");
                } else if ($(this).val() == 'push') {
                    $('#app-header, #app-container').attr("app-lpanel-effect", "push");
                } else {
                    $('#app-header, #app-container').attr("app-lpanel-effect", "shrink");
                }
            });

        },

        Handlethemeoption: function () {
            $('.selector-toggle > a').on("click", function () {
                $('#styleSelector').toggleClass('open')
            });

        },
        Handlelpanelmenu: function () {
            $('.app-has-menu > a').on("click", function () {
                var compactMenu = $(this).closest('.app-minimized-lpanel').length;
                if (compactMenu === 0) {
                    $(this).parent('.app-has-menu').parent('ul').find('ul:visible').slideUp('fast');
                    $(this).parent('.app-has-menu').parent('ul').find('.opened').removeClass('opened');
                    var submenu = $(this).parent('.app-has-menu').find('>.app-sub-menu');
                    if (submenu.is(':hidden')) {
                        submenu.slideDown('fast');
                        $(this).parent('.app-has-menu').addClass('opened');
                    } else {
                        $(this).parent('.app-has-menu').parent('ul').find('ul:visible').slideUp('fast');
                        $(this).parent('.app-has-menu').removeClass('opened');
                    }
                }
            });

        },
        HandleSidebartoggle: function () {
            $('.app-sidebar-toggle a').on("click", function () {
                if ($('#app-wrapper').attr("app-device-type") !== "phone") {
                    $('#app-container').toggleClass('app-minimized-lpanel');
                    $('#app-header').toggleClass('app-minimized-lpanel');
                    if ($('body').attr("app-navigation-type") !== "vertical-compact") {
                        $('body').attr("app-navigation-type", "vertical-compact");
                    } else {
                        $('body').attr("app-navigation-type", "vertical");
                    }
                } else {
                    if (!$('#app-wrapper').hasClass('app-hide-lpanel')) {
                        $('#app-wrapper').addClass('app-hide-lpanel');
                    } else {
                        $('#app-wrapper').removeClass('app-hide-lpanel');
                    }
                }
            });

        },
        Handlelpanel: function () {

            function Responsivelpanel() {

                var totalwidth = $(window)[0].innerWidth;
                if (totalwidth >= 768 && totalwidth <= 1024) {
                    $('#app-wrapper').attr("app-device-type", "tablet");
                    $('#app-header, #app-container').addClass('app-minimized-lpanel');
                    $('li.theme-option select').attr('disabled', false);
                } else if (totalwidth < 768) {
                    $('#app-wrapper').attr("app-device-type", "phone");
                    $('#app-header, #app-container').removeClass('app-minimized-lpanel');
                    $('li.theme-option select').attr('disabled', 'disabled');
                } else {
                    if ($('body').attr("app-navigation-type") !== "vertical-compact") {
                        $('#app-wrapper').attr("app-device-type", "desktop");
                        $('#app-header, #app-container').removeClass('app-minimized-lpanel');
                        $('li.theme-option select').attr('disabled', false);
                    } else {
                        $('#app-wrapper').attr("app-device-type", "desktop");
                        $('#app-header, #app-container').addClass('app-minimized-lpanel');
                        $('li.theme-option select').attr('disabled', false);

                    }
                }
            }
            Responsivelpanel();
            $(window).resize(Responsivelpanel);

        },

    };
    AppDatapp.appinit();
});

$("#death").click(function () {
    $("#deathnone").toggle('show');
});


$("#imuyes").click(function () {
    $("#imunone").toggle();
});


$("#imuno").click(function () {
    $("#imunonone").toggle();
});

$('#isNews').change(function () {
    $("#newsSource").prop("disabled", !$(this).is(':checked'));
});


//// Date Time Function

//tday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
//tmonth = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
tday = new Array("Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat");
tmonth = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec");

function GetClock() {
    var d = new Date();
    var nday = d.getDay(), nmonth = d.getMonth(), ndate = d.getDate(), nyear = d.getYear();
    if (nyear < 1000) nyear += 1900;
    var nhour = d.getHours(), nmin = d.getMinutes(), nsec = d.getSeconds(), ap;

    if (nhour == 0) { ap = " AM"; nhour = 12; }
    else if (nhour < 12) { ap = " AM"; }
    else if (nhour == 12) { ap = " PM"; }
    else if (nhour > 12) { ap = " PM"; nhour -= 12; }

    if (nmin <= 9) nmin = "0" + nmin;
    if (nsec <= 9) nsec = "0" + nsec;

    document.getElementById('datetime').innerHTML = "" + tday[nday] + ", " + tmonth[nmonth] + " " + ndate + ", " + nyear + " " + nhour + ":" + nmin + ":" + nsec + ap + "";
}

window.onload = function () {
    GetClock();
    setInterval(GetClock, 1000);
}
