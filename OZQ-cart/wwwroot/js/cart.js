$("document").ready(async function () {
  if (!window.cartData) {
    window.cartData = {};
  }
  var intervalIdnew = setInterval(() => {
    if (window.customerId) {
      clearInterval(intervalIdnew);
      $.ajax({
        type: "GET",
        url: "./api/CartItems/customer/" + window.customerId,
        contentType: "application/json",
        success: function (data) {
          $.each(data, function (index, item) {
            cartData[item.itemId] = item.quantity;
          });
          console.log(cartData);
        },
        error: function (data) {
          console.log("Error getting cart data");
        },
      });
    }
  }, 100);
  var cartContainer = document.getElementById("cartContainer") || null;
  var checkoutContainer = document.getElementById("checkoutContainer") || null;
  let intervalId = setInterval(async () => {
    if (window.customerId) {
      clearInterval(intervalId);
      try {
        let cartDataResponse = await $.ajax({
          type: "GET",
          url: "./api/CartItems/customer/" + window.customerId,
          contentType: "application/json",
        });

        console.log("cart data");
        console.log(cartDataResponse);

        for (let i = 0; i < cartDataResponse.length; i++) {
          console.log(cartDataResponse[i]);
          var cartId = cartDataResponse[i].id;
          var itemId = cartDataResponse[i].itemId;
          var quantity = cartDataResponse[i].quantity;

          console.log("cartId old:" + cartId);

          try {
            let itemDataResponse = await $.ajax({
              type: "GET",
              url: "./api/Items/" + itemId,
              contentType: "application/json",
            });

            console.log("cart data and quantity" + quantity);
            console.log("cartId:" + cartId);
            console.log(itemDataResponse);
            var cartItem = `<div class="row" style="background-color: rgb(245, 245, 239); margin-inline: 20px;margin-block: 10px; padding-inline: 50px;border-radius: 20px;">
                    <div class="col-2">
                        <img src="${
                          itemDataResponse.image
                        }" alt="" style="display: block;
                    width: 100px;
                    height: 100px;
                    object-fit: cover;border-radius: 100px;">
                    </div>
                    <div class="col-9" style="margin-top: auto; padding-left: 50px;">
                        <h5>${itemDataResponse.name}</h5>
                        <p style="color: darkslategrey;">Item Price: Rs. ${
                          itemDataResponse.price
                        } <span style="margin-left:20px;">Quantity:
                                ${quantity}</span> </p>
      
                    </div>
                    <div class="totalPrice" style="display: none;">${
                      itemDataResponse.price * quantity
                    }</div>
      
                    <div class="col-1">
                        <button onclick="removeCartItem(${cartId})" class="btn btn-danger" style="margin-top: 20px;">X</button>
                    </div>
                </div>`;
            cartContainer.innerHTML += cartItem;
            if (checkoutContainer) {
              checkoutContainer.innerHTML += cartItem;
            }
          } catch (error) {
            console.log("no item data");
            console.log(error);
          }
        }
      } catch (error) {
        console.log("no data");
        console.log(error);
      }
    }
  }, 100);
});

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

function removeCartItem(id) {
  console.log("remove cart item");
  $.ajax({
    type: "DELETE",
    url: "./api/CartItems/" + id,
    contentType: "application/json",
    success: function (data, status, xhr) {
      alert("cart item deleted");
      //retrieve token from the response and store it
      console.log(data);
      location.reload();
    },
    error: function (data, xhr, status, error) {
      console.log("no data");
      console.log(data.responseJSON);
    },
  });
}

function addSingleCartItem(itemId) {
  if (window.customerId != "" && window.customerId != null) {
    $.ajax({
      type: "POST",
      url: "./api/CartItems",
      contentType: "application/json",
      data: JSON.stringify({
        itemId: itemId,
        customerId: window.customerId,
        quantity: 1,
      }),
      success: function (data, status, xhr) {
        console.log("cart item added");
        console.log("add single cart item");
        alert("item added to cart");
        //retrieve token from the response and store it
        console.log(data);
        location.reload();
      },
      error: function (data, xhr, status, error) {
        console.log("no data");
        console.log(data.responseJSON);
      },
    });
  } else {
    alert("Login required to add item to cart");
  }
}
