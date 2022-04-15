window.onload = function () {
    let remove = document.getElementsByClassName("btn-danger");
    let quantity = document.getElementsByClassName("btn-secondary");
    let elem = document.getElementById("checkoutBtn");

    elem.addEventListener('click', ShowModal);

    for (let i = 0; i < remove.length; i++) {
        remove[i].addEventListener('click', Removeclick);
    }

    //for (let i = 0; i < quantity.length; i++) {
    //    quantity[i].addEventListener('input', UpdateQuantityCart);
    //}

    for (let i = 0; i < quantity.length; i++) {
        quantity[i].addEventListener('click', UpdateClick);
    }

    function Removeclick(event) {
        RemoveFromCart(event.target.value);
    }


    ////keeping track using mouse leaving the table or when user clicks outside the button 
    //function UpdateClick(event) { 
    //    var specifiedElement = document.getElementById(event.target.id)
    //    var carttable = document.getElementById('carttable') 

    //    carttable.addEventListener('mouseleave', function (event) {
    //        UpdateCartQty(specifiedElement.id, specifiedElement.value)
    //    })

    //    document.addEventListener('click', function (event) {
    //        var isClickInside = specifiedElement.contains(event.target);

    //        if (!isClickInside) {
    //            UpdateCartQty(specifiedElement.id,specifiedElement.value)
    //        }
    //    });
    //}


   //Keeping track using mouse leaving the specified row
    function UpdateClick(event) {
        var specifiedRow = document.getElementById(event.target.getAttribute('data-value'))
        var specifiedElement = document.getElementById(event.target.id)

        specifiedRow.addEventListener('mouseleave', function (event){
            UpdateCartQty(specifiedElement.id, specifiedElement.value)
        })
    }



}


function UpdateCartQty(id ,value) {

    var Value = value * 1;

    if (Value < 1 || !Number.isInteger(Value)) {
        document.getElementById(id).value = 1;
        Value = document.getElementById(id).value * 1;
        swal({
            icon: "warning",
            title: "Please input a correct quantity.",
            text: "You may remove the item by clicking on the delete icon on the right."
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
        text: "The stock might run out!",
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




//function UpdateQuantityCart(event) {
//    let productId = event.target.id;
//    let shopcartitemid = event.target.getAttribute('data-value');
//    let ProductpriceId = "ProductPrice" + productId;

//    let value = document.getElementById(productId).value * 1;

//    let Productprice = parseFloat(document.getElementById(ProductpriceId).innerHTML.substring(1));

//    if (value < 1 || !Number.isInteger(value)) {
//        alert("Please input a correct quantity. You may remove the item by clicking on the delete icon on the right.");
//        document.getElementById(productId).value = 1;
//    }

//    value = document.getElementById(productId).value * 1;

//    UpdateCartItemwithQuantity(productId, value, shopcartitemid);
//}

//function UpdateCartItemwithQuantity(productId, value, shopcartitemid) {
//    let xhr = new XMLHttpRequest();

//    xhr.open("POST", "/CheckOut/UpdateQuantity");

//    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

//    xhr.onreadystatechange = function () {
//        if (this.readyState == XMLHttpRequest.DONE) {
//            if (this.status == 200) {
//                let data = JSON.parse(this.responseText);


//                if (data.status == "success") {
//                    window.location.reload(true);
//                }
//            }
//        }
//    }

//    let shopcartitem = {
//        Id: shopcartitemid,
//        ProductId: productId,
//        Quantity: value

//    }

//    xhr.send(JSON.stringify(shopcartitem));
//}

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
                    swal({
                        title: "Thank You!",
                        text: "Your purchase was successful.",
                        icon: "success"
                    });
                    window.location.href = '/CheckOut/CheckOutCart/'
                }
            }
        }
    };
    xhr.send()
}