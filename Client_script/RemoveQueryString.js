$(document).ready(function () {    
    let url = window.location.href;
    var splitUrl = url.split('?');
    if (splitUrl.length > 1) {
        let params = new URLSearchParams(splitUrl[1]);

        params.delete("cpat");
        if (params.toString() != "")
            ChangeUrl('Url', splitUrl[0] + '?' + params);
        else
            ChangeUrl('Url', splitUrl[0] + params);
    }

});


function ChangeUrl(title, url) {
    if (typeof (history.pushState) != "undefined") {
        var obj = { Title: title, Url: url };
        history.pushState(obj, obj.Title, obj.Url);
    } else {
        alert("Browser does not support HTML5.");
    }
}