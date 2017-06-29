

var imagePreview = function (newConfig) {
    /* CONFIG */

    var imageCloseOff = false;
    var config = {

        xOffset: 100,
        yOffset: 50,
        width: 600,
        height: 800,
        isLoader: false,
        preloadTime: 500
    }
    //if (newConfig != null) {
    //    $.extend(config, newConfig);
    //}
    /* END CONFIG */

    $(document).on('mouseover', '.preview', function (e) {

        $("body").append("<p id='preview'><span style='display:block;width:200px;'>" + $(this).attr('alt') + "</span><img class='prew-img'  style='display:none;max-width:600px;' src='" + $(this).attr('src') + "' alt='Image preview' /></p>");
        $("#preview")
        .css("top", (e.pageY - 50) + "px")
        .css("left", (e.pageX + 50) + "px")
        .fadeIn("fast");
        $('.prew-loader').remove(); $('#preview img').fadeIn('slow');
    });
    $(document).on('mouseout', '.preview', function (e) {
        $('#preview').remove();
    });
    $("img.preview").mousemove(function (e) {

        var thisImg = $(this);

        setTimeout(function () { $('.prew-loader').remove(); $('#preview').css({ minWidth: thisImg.data('w') + "px", minHeight: thisImg.data('h') + "px" }); }, config.preloadTime);
        if ($(this).hasClass('gal-img-item')) config.xOffset = 350;

        if (e.pageX > 650) {

            $("#preview")
        .css("top", (e.pageY - config.xOffset) + "px")
        .css("left", (e.pageX - (config.yOffset + config.width)) + "px");

        }
        else if (e.pageY > 400) {
            $("#preview")
                .css("top", (e.pageY - 350) + "px")
                .css("left", (e.pageX + config.yOffset) + "px");
        }
        else {
            $("#preview")
         .css("top", (e.pageY - config.xOffset) + "px")
         .css("left", (e.pageX + config.yOffset) + "px");
        }
    });
};

//window.on('.cke_body', function () {
//    var h = $('#admincontent').innerHeight();
//    $('#categories').css({ 'height': h });
//});
