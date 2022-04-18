
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
    swal(copyText.value + " Activation code copied");
}

function copySingle(index) {
    var copyText = document.getElementById(index);
    navigator.clipboard.writeText(copyText.innerText);
    swal(copyText.innerText + " Activation code copied");
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
                var date = "<b>Date:</b>  " + data.date;
                var rating = "<b>Rating:</b> " + data.rating + "/5";
                var content = data.content;
                if (content != "") {
                    content = "</p><p><b>Content:</b><br>" + content;
                }
                else {
                    content = "There is no content in this review!"
                }
                obj.innerHTML = "<h4 class='mb-3'>Your Review...</h4>" +
                    "<div class='border p-3'>" +
                    "<p>" + date + "</p>" +
                    "<p>" + rating + "</p>" +
                    "<p>" + content + "</p></div>";
            }
        }
    }
    let data = {
        "Id": orderId
    };
    xhr.send(JSON.stringify(data));
}
//added function of user's comments validation
window.onload = function () {
    let elems = document.getElementsByClassName("reviewdetail");
    for (let i = 0; i < elems.length; i++) {
        elems[i].addEventListener('click', Clickfordetail);
    }
    let form = document.getElementById("reviewform");
    form.onsubmit = function () {
        let rate = document.getElementById("rate");
        let content = document.getElementById("content");

        event.preventDefault();
        if (content.value.length > 250) {
            swal({
                icon: "warning",
                text: "Sorry! Max 250 characters please!"
            });
            return false;
        }
        else {
            swal({
                icon: "success",
                title: "Thank You!",
                text: "Your review has been submitted.",
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK",
                closeOnClickOutside: false,
                closeOnEsc: false,
            }).then(
                function (isConfirmed) {
                    {
                        if (isConfirmed) {
                            form.submit();
                        }
                    }
                })
        }
    }
}



function Clickfordetail(event) {
    showreview(event.target.id);
}


//function to keep track of Cart Counter
$(document).ready(function () {
    setInterval(reload, 1000);
})

function reload() {
    $("#here").load(window.location.href + " #here");
}
