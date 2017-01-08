$("document").ready(function () {


    //usunięcie komentarza
    $(".commentBtnDelete").click(function (e) {
        deletePostComment(this, "GET", "#commentPanel_", e);
    });//end commnetBtnDelete Click

    //usunięcie posta
    $(".postBtnDelete").click(function (e) {
        deletePostComment(this, "POST", "#postPanel_", e);
    });//end postBtnDelete Click


    //==== HELPER FUNCTION ====
    function deletePostComment(element, typeMethod, removeElement, event) {
        event.preventDefault();

        var url = $(element).attr("href");

        var id = url.substr(url.lastIndexOf("/") + 1, url.length - url.lastIndexOf("/"));

        $.ajax({
            type: typeMethod,
            url: url,
            data: {},
            datatype: "html",
            success: function (data) {
                alert(data.message);
                $(removeElement + id).remove();
            }
        });//end ajax
    }//end deletePostComment
    //==== HELPER FUNCTION ====

});// end document ready