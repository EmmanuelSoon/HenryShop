
$('.carousel-control-prev').click(function () {
    $('#mycarousel').carousel('prev');
});

$('.carousel-control-next').click(function () {
    $('#mycarousel').carousel('next');
});

$('a[data-slide="prev"]').click(function () {
    $('#mycarousel').carousel('prev');
});

$('a[data-slide="next"]').click(function () {
    $('#mycarousel').carousel('next');
});

function copy(index) {
    var copyText = document.getElementById(index);
    navigator.clipboard.writeText(copyText.value);
    alert("Copied the Code: " + copyText.value);
}

function copySingle(index) {
    var copyText = document.getElementById(index);
    navigator.clipboard.writeText(copyText.innerText);
    alert("Copied the Code: " + copyText.innerText);
}