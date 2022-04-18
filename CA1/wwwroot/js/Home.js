window.onload = function () {

    let add = document.getElementsByClassName("btn-outline-primary");
    let addwishlists = document.getElementsByClassName("fa-heart");
    let deleteWL = document.getElementsByClassName("btn btn-outline-danger");

    for (let i = 0; i < add.length; i++) {
        add[i].addEventListener('click', Addclick);
    }
    for (let i = 0; i < addwishlists.length; i++) {
        addwishlists[i].addEventListener('click', Addlist);
    }
    for (let i = 0; i < deleteWL.length; i++) {
        deleteWL[i].addEventListener('click', DeleteClick);
    }


    function Addclick(event) {
        AddToCart(event.target.value);
    }
    function Addlist(event) {
        AddToWishList(event.target.value);
    }
    function DeleteClick(event) {
        RemoveFromWishList(event.target.value);
    }

}



function AddToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Search/AddToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {

                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    $('body').append(
                        '<div></div><div id="myModal" class="modal fade" role="dialog">' +
                        '  <div class="modal-dialog">' +
                        '    <div class="modal-content">' +
                        '      <div class="modal-header">' +
                        '        <h3 class="modal-title">Success!</h3>' +
                        '        <button type = "button" class= "close" data-dismiss="modal">&times;</button>' +
                        '      </div>' +
                        '      <div class="modal-body">' +
                        '           <p>' + data.name + ' Added to cart' +
                        '</p>' +
                        '      </div>' +
                        '      <div class="modal-footer">' +
                        '        <button type="button" class="btn btn-secondary" data-dismiss="modal">Continue Shopping</button>' +
                        '       <a href="/CheckOut" class="btn btn-primary" role="button">Go To Cart</a>' +
                        '      </div>' +
                        '    </div>' +
                        '  </div>' +
                        '</div>');
                    $('#myModal').modal('show');

                    setTimeout(function () {
                        $(myModal).modal('hide');
                    }, 2000);

                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}

function AddToWishList(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Wishlist/AddToWishList");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {
                    swal(data.name + " Added to your wishlist.");
                }
                else if (data.status == "existed") {
                    swal({
                        text: data.name + " Already exists in your wishlist.",
                        icon: "info"
                    });
                }
                else if (data.status == "needlogin") {
                    $('#modalLoginForm').modal('toggle');
                }
            }
        }
    }

    let req = { "Id": id }
    xhr.send(JSON.stringify(req));
}

function RemoveFromWishList(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Wishlist/RemoveFromWishList");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                if (data.status == "success") {

                    swal({
                        text: "Item is removed from your wishlist.",
                        icon: "info"
                    }).then(function (){
                        window.location = '/WishList';
                    });
                   
                }
                else if (data.status == "error") {
                    
                    swal({
                        icon: "error",
                        text: "There was an error removing the item."
                    }).then(function () {
                        window.location = '/WishList';
                    });
                }
            }
        }
    }

    let delReq = { "Id": id }
    xhr.send(JSON.stringify(delReq));
}

