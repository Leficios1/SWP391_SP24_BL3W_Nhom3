import 'package:electronic_equipment_store/core/constants/color_constants.dart';
import 'package:electronic_equipment_store/models/product_model.dart';
import 'package:electronic_equipment_store/representation/screens/customer/checkout.dart';
import 'package:electronic_equipment_store/representation/screens/widgets/app_bar_main.dart';
import 'package:electronic_equipment_store/services/auth_provider.dart';
import 'package:electronic_equipment_store/services/cart_provider.dart';
import 'package:electronic_equipment_store/utils/asset_helper.dart';
import 'package:electronic_equipment_store/utils/image_helper.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class MainCartScreen extends StatefulWidget {
  const MainCartScreen({super.key});

  static const String routeName = '/main_cart_screen';

  @override
  // ignore: library_private_types_in_public_api
  _MainCartScreenState createState() => _MainCartScreenState();
}

class _MainCartScreenState extends State<MainCartScreen> {
  double totalAmount = 0.0;
  int totalSelectedProducts = 0;
  List<ProductModel> cartItems = [];

  @override
  Widget build(BuildContext context) {
    final cartProvider = Provider.of<CartProvider>(context);
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    cartItems = cartProvider.cartItems;

    totalSelectedProducts = 0;
    totalAmount = 0.0;
    for (var product in cartItems) {
      totalSelectedProducts++;
      totalAmount += product.price *
          product
              .quantityUserWantBuy!; // Thay đổi tính toán tổng số tiền dựa trên số lượng sản phẩm
    }

    return AppBarMain(
      leading: ImageHelper.loadFromAsset(AssetHelper.imageLogo),
      titleAppbar: "ELECTRICITY STORE",
      child: Scaffold(
          body: authProvider.isLoggedIn
              ? cartItems.isEmpty
                  ? const Center(
                      child: Text(
                        "Giỏ hàng của bạn chưa có sản phẩm!",
                        style: TextStyle(fontSize: 18),
                      ),
                    )
                  : Column(
                      children: [
                        const SizedBox(height: 20),
                        Expanded(
                          child: ListView.builder(
                            itemCount: cartItems.length,
                            itemBuilder: (context, index) {
                              final cartItem = cartItems[index];
                              return Container(
                                padding: const EdgeInsets.all(5),
                                margin: const EdgeInsets.only(bottom: 20),
                                decoration: BoxDecoration(
                                  color: Colors.white,
                                  borderRadius: BorderRadius.circular(14),
                                ),
                                child: ListTile(
                                  leading: CircleAvatar(
                                    backgroundImage: NetworkImage(
                                        cartItem.productImage ?? ''),
                                  ),
                                  title: Text(cartItem.productName),
                                  subtitle: Column(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: [
                                      Text(
                                        NumberFormat.currency(
                                                locale: 'vi_VN', symbol: 'vnđ')
                                            .format(cartItem.price),
                                        style:
                                            const TextStyle(color: Colors.red),
                                      ),
                                      const SizedBox(height: 10),
                                      Row(
                                        children: [
                                          const Text('Số lượng: '),
                                          IconButton(
                                            icon: const Icon(Icons.remove, color: ColorPalette.primaryColor,),
                                            onPressed: () {
                                              if (cartItem
                                                      .quantityUserWantBuy! >
                                                  1) {
                                                setState(() {
                                                  cartItem.quantityUserWantBuy =
                                                      cartItem.quantityUserWantBuy! -
                                                          1;
                                                });
                                                cartProvider
                                                    .updateQuantityProductInCart(
                                                        cartItem,
                                                        cartItem
                                                            .quantityUserWantBuy!);
                                              }
                                            },
                                          ),
                                          Text(
                                              '${cartItem.quantityUserWantBuy}'),
                                          IconButton(
                                            icon: const Icon(Icons.add, color: ColorPalette.primaryColor,),
                                            onPressed: () {
                                              setState(() {
                                                cartItem.quantityUserWantBuy =
                                                    cartItem.quantityUserWantBuy! +
                                                        1;
                                              });
                                              cartProvider
                                                  .updateQuantityProductInCart(
                                                      cartItem,
                                                      cartItem
                                                          .quantityUserWantBuy!);
                                            },
                                          ),
                                        ],
                                      ),
                                    ],
                                  ),
                                  trailing: IconButton(
                                    icon: const Icon(Icons.delete, color: ColorPalette.primaryColor,),
                                    onPressed: () {
                                      cartProvider
                                          .removeProductFromCart(cartItem);
                                    },
                                  ),
                                ),
                              );
                            },
                          ),
                        ),
                        const SizedBox(height: 20),
                        Container(
                          padding: const EdgeInsets.all(10),
                          decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(14),
                            color: Colors.white,
                          ),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.stretch,
                            children: [
                              const Text(
                                'Tổng thanh toán',
                                style: TextStyle(fontWeight: FontWeight.bold),
                              ),
                              const SizedBox(height: 10),
                              Text('$totalSelectedProducts sản phẩm'),
                              const SizedBox(height: 10),
                              const Divider(color: Colors.black),
                              const SizedBox(height: 10),
                              Text(
                                NumberFormat.currency(
                                        locale: 'vi_VN', symbol: 'vnđ')
                                    .format(totalAmount),
                                style: const TextStyle(
                                    color: Colors.red,
                                    fontSize: 18,
                                    fontWeight: FontWeight.bold),
                              ),
                              const SizedBox(height: 10),
                              ElevatedButton(
                                style: ButtonStyle(
                                  backgroundColor:
                                      MaterialStateProperty.all<Color>(
                                          ColorPalette.primaryColor),
                                  foregroundColor:
                                      MaterialStateProperty.all<Color>(
                                          Colors.white),
                                ),
                                onPressed: () {
                                  Navigator.of(context).push(CupertinoPageRoute(
                                      builder: ((context) =>
                                          const Checkout())));
                                },
                                child: const Text('Đặt hàng'),
                              ),
                            ],
                          ),
                        ),
                      ],
                    )
              : const Center(
                  child: Padding(
                    padding: EdgeInsets.all(20.0),
                    child: Text(
                      "Vui lòng đăng nhập để sử dụng tính năng này!",
                      style: TextStyle(fontSize: 18),
                      textAlign: TextAlign.center,
                    ),
                  ),
                )),
    );
  }
}
