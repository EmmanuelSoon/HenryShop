window.onload = function () {
    let remove = document.getElementsByClassName("btn-danger");
    let quantity = document.getElementsByClassName("btn-secondary");
    let elem = document.getElementById("checkoutBtn");
    let change = document.getElementsByClassName("btn-warning");

    elem.addEventListener('click', ShowModal);

    for (let i = 0; i < remove.length; i++) {
        remove[i].addEventListener('click', Removeclick);
    }

    for (let i = 0; i < quantity.length; i++) {
        quantity[i].addEventListener('click', UpdateClick);
    }

    for (let i = 0; i < change.length; i++) {
        change[i].addEventListener('click', ChangeClick);
    }

    function ChangeClick(event) {
        ChangeQ(event.target.value, event.target.getAttribute('data-value'));
    }

    function Removeclick(event) {
        RemoveFromCart(event.target.value);
    }

    //Keeping track using mouse leaving the specified button
    function UpdateClick(event) {
        var specifiedElement = document.getElementById(event.target.id)

        specifiedElement.addEventListener('mouseleave', function (event) {
            UpdateCartQty(specifiedElement.id, specifiedElement.value)
        })
    }
}


function ChangeQ(id, qty) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/CheckOut/ChangeQ");
    xhr.setRequestHeader("Content-type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {
                    swal({
                        icon: "info",
                        title: "Item Quantity Changed!",
                        text: "It has been changed to " + qty,
                    }).then(function () {
                        window.location.reload(true)
                    })
                }
                else {
                    swal({
                        icon: "warning",
                        title: "Item was Removed",
                        text: "Sorry we are out of stock..."
                    }).then(function () {
                        window.location.reload(true);
                    })
                }
            }
        }
    };

    let req = {
        "Id": id,
        Quantity: parseInt(qty),
    }

    xhr.send(JSON.stringify(req));
}


async function UpdateCartQty(id, value) {

    var Value = value * 1;

    if (Value < 1 || !Number.isInteger(Value)) {

        document.getElementById(id).value = 1;
        Value = document.getElementById(id).value * 1;
        await swal({
            icon: "warning",
            title: "Please input a correct quantity.",
            text: "You may remove the item by clicking on the delete icon on the right.",
            closeOnConfirm: false,
            closeOnCancel: false,
            allowOutsideClick: false,

        });
    }

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/UpdateQuantity");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }
    let req = {
        "Id": id,
        "Quantity": Value
    }

    xhr.send(JSON.stringify(req));
}



function RemoveFromCart(id) {

    swal({
        title: "Remove item from Cart?",
        text: "Click OK to confirm...",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            let xhr = new XMLHttpRequest();

            xhr.open("POST", "/CheckOut/RemoveFromCart");

            xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

            xhr.onreadystatechange = function () {
                if (this.readyState == XMLHttpRequest.DONE) {
                    if (this.status == 200) {
                        let data = JSON.parse(this.responseText);


                        if (data.status == "success") {
                            window.location.reload(true);
                        }
                    }
                }
            }

            let req = { "Id": id }

            xhr.send(JSON.stringify(req));
        }
    });

}



//show login modal
function ShowModal(event) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Checkout/CheckUser");
    xhr.setRequestHeader("Content-type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "notlogged") {
                    $('#modalLoginForm').modal('toggle');
                }
                else {
                    CheckOutCart();
                }
            }
        }
    };
    xhr.send()
}

function CheckOutCart() {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Checkout/CheckOutCart");
    xhr.setRequestHeader("Content-type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    swal({
                        title: "Thank You!",
                        text: "Your purchase was successful.",
                        icon: "success"
                    }).then(function () {
                        window.location = '/Purchase/Index/';
                    });
                }
                if (data.status == "fail") {
                    window.location.reload(true);
                }
            }
        }
    };
    xhr.send()
}