function main() {
    initNavigationMenu();
}

function initNavigationMenu() {
    $("#navigation-menu").menu();

    $(".menu-item-link").click(function () {
        loadPageContent(this);
    });

    loadPageContent($(".menu-item-link[pageName=home]").get(0));
}

function loadPageContent(element) {
    var pageUrl = $(element).attr("pageName") + ".html";
    $.get(pageUrl, function (pageContent) {
        $("#content-div").html(pageContent);
        $('code').each(function (i, e) { hljs.highlightBlock(e) });
    });
}