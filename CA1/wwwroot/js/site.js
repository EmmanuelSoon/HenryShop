
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

function showreview(orderId) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Purchase/ReviewDetail");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status != 200) {
                return;
            }
            let data = JSON.parse(this.responseText);
            if (data.status === "success") {
                let obj = document.getElementById(orderId);
                var date = "Review was made on " + data.date;
                var rating = "You rated " + data.rating + "/5";
                var content = data.content;
                obj.innerHTML = "<p>" + date + "</p><p>" + rating + "</p><p><b>Content:</b><br>" + content + "</p>";
            }
        }
    }
    let data = {
        "Id": orderId
    };
    xhr.send(JSON.stringify(data));
}
window.onload = function () {
    let elems = document.getElementsByClassName("reviewdetail");
    for (let i = 0; i < elems.length; i++) {
        elems[i].addEventListener('click', Clickfordetail);
    }
}
function Clickfordetail(event) {
    showreview(event.target.id);
}