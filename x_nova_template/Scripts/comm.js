
$(function () {
    $modal = $('#modal-6');
    $auth = $('#modal-7');
    Element.prototype.remove = function () {
        this.parentElement.removeChild(this);
    }
    NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
        for (var i = 0, len = this.length; i < len; i++) {
            if (this[i] && this[i].parentElement) {
                this[i].parentElement.removeChild(this[i]);
            }
        }
    }
    jQuery.fn.center = function () {
        this.closest('div').css('position', 'relative');
        this.css("position", "absolute");

        var wh = Math.max(this.closest('div').outerHeight() - this.height()) / 2;
        var he = Math.max(this.closest('div').outerWidth() - this.width()) / 2;

        this.css('top', wh + "px");
        this.css('left', he + "px");

        return this;
    }
    jQuery.fn.cleanWhitespace = function () {
        textNodes = this.contents().filter(
            function () { return (this.nodeType == 3 && !/\S/.test(this.nodeValue)); })
            .remove();
        return this;
    }
    jQuery.fn.addloader = function () {
        var wrap = '<span class="md-loader"><img src="/Content/ajax-loaders/5.GIF" /></span>';
        this.html(wrap);

    }
    //alert($('#screenWidth').val());
   
});

//$(function () {
//    var menuLeft = $('#cbp-spmenu-s1'),
//    //menuRight = document.getElementById('cbp-spmenu-s2'),
//    //menuTop = document.getElementById('cbp-spmenu-s3'),
//    //menuBottom = document.getElementById('cbp-spmenu-s4'),
//    showLeft = $('#showLeft'),
//    //showRight = document.getElementById('showRight'),
//    //showTop = document.getElementById('showTop'),
//    //showBottom = document.getElementById('showBottom'),
//    //showLeftPush = document.getElementById('showLeftPush'),
//    //showRightPush = document.getElementById('showRightPush'),
//    body = $('body');
//    body.on('click', function () {
//        if (menuLeft.hasClass('cbp-spmenu-open')) { menuLeft.toggleClass('cbp-spmenu-open'); showLeft.toggleClass('active'); }
//    });
//    showLeft.on('click', function () {

//        $(this).toggleClass('active');
//        setTimeout(function () { menuLeft.toggleClass('cbp-spmenu-open'); }, '100');
//        //disableOther('showLeft');
//    });
//    //showRight.onclick = function () {
//    //    classie.toggle(this, 'active');
//    //    classie.toggle(menuRight, 'cbp-spmenu-open');
//    //    disableOther('showRight');
//    //};
//    //showTop.onclick = function () {
//    //    classie.toggle(this, 'active');
//    //    classie.toggle(menuTop, 'cbp-spmenu-open');
//    //    disableOther('showTop');
//    //};
//    //showBottom.onclick = function () {
//    //    classie.toggle(this, 'active');
//    //    classie.toggle(menuBottom, 'cbp-spmenu-open');
//    //    disableOther('showBottom');
//    //};
//    //showLeftPush.onclick = function () {
//    //    classie.toggle(this, 'active');
//    //    classie.toggle(body, 'cbp-spmenu-push-toright');
//    //    classie.toggle(menuLeft, 'cbp-spmenu-open');
//    //    disableOther('showLeftPush');
//    //};
//    //showRightPush.onclick = function () {
//    //    classie.toggle(this, 'active');
//    //    classie.toggle(body, 'cbp-spmenu-push-toleft');
//    //    classie.toggle(menuRight, 'cbp-spmenu-open');
//    //    disableOther('showRightPush');
//    //};

//    function disableOther(button) {
//        if (button !== 'showLeft') {
//            classie.toggle(showLeft, 'disabled');
//        }
//        //if (button !== 'showRight') {
//        //    classie.toggle(showRight, 'disabled');
//        //}
//        //if (button !== 'showTop') {
//        //    classie.toggle(showTop, 'disabled');
//        //}
//        //if (button !== 'showBottom') {
//        //    classie.toggle(showBottom, 'disabled');
//        //}
//        //if (button !== 'showLeftPush') {
//        //    classie.toggle(showLeftPush, 'disabled');
//        //}
//        //if (button !== 'showRightPush') {
//        //    classie.toggle(showRightPush, 'disabled');
//        //}
//    }
//});


