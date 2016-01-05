
$(function () {
    /*fmenu*/

    var fcont = $('.foreign-cont');
    $('.forg-btn').hover(function (e) {


        fcont.css({ marginTop: '20px', opacity: 1, visibility: '' });

    }, function () { $('.foreign-cont').css({ marginTop: '50px', opacity: 0, visibility: 'hidden' }); });

    $(document).on('mouseover', '.foreign-cont,.forg-tnl', function () {
        $('.foreign-cont').css({ marginTop: '20px', opacity: 1, visibility: '' });
    });
    $(document).on('mouseout', '.foreign-cont,.forg-tnl', function () {
        fcont.css({ marginTop: '50px', opacity: 0, visibility: 'hidden' });
    });
    /* tmenu */
    var tlink = $('.tmenu li a');
    tlink.hover(function () {

        $(this).addClass('no-pseudo');
    }, function () { $(this).removeClass('no-pseudo'); });

});

