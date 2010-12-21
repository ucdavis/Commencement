function SetScrollingBox(fixedBoxId, inlineBoxId) {
    this.fixedBox = $("#" + this.options.fixedBoxId);
    this.inlineBox = $("#" + this.options.inlineBoxId);

    if (this.fixedBox == undefined || this.inlineBox == undefined) {
        // this is a problem and shouldn't work
        return;
    }

    // hide the fixed box
    this.fixedBox.hide();

    // set the scroll event handler
    $(window).scroll(function () {
        var scrollPosition = $(document).scrollTop();   // current window's scroll position
        var obj = $("#inlinebox");
        var box = obj[0];                               // actual box object

        // scrolled past the box
        if (scrollPosition > box.offsetHeight + box.offsetTop) {
            $("#scrollingbox").fadeIn(500);
        }
        // box is in view
        else {
            $("#scrollingbox").fadeOut(500);
        }
    });
}