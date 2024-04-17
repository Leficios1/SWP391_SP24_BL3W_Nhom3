import { Route, Routes } from "react-router-dom"
import Home from "../modules/home/home"
import ErrorPage from "../shared/error/error"
import ProductDetail from "../entities/product-detail/ProductDetail"
import Login from "../modules/login/login"
import Header from "../shared/layout/header/header"
import Footer from "../shared/layout/footer"
import { Button, Divider, FloatButton, Popover, Row } from "antd"
import { Cart, CartWithoutLogin } from "../entities/cart/Cart"
import { DeleteOutlined, ShoppingCartOutlined } from "@ant-design/icons"
import { useEffect, useState } from "react"
import Cookies from "universal-cookie"
import { useAppDispatch, useAppSelector } from "../config/store"
import { getCartByUserId } from "../entities/cart/cart.reducer"
import "./routes.scss"

const AppRoutes = () => {
    const [isOpenCart, setIsOpenCart] = useState<boolean>(false)
    const cookie = new Cookies();
    const account = cookie.get("account");
    const dispatch = useAppDispatch()
    const productsInCart = useAppSelector(state => state.cart.dataDetail);
    const isLoading = useAppSelector(state => state.cart.loading);

    useEffect(() => {
        if (account != null) {
            dispatch(getCartByUserId(account.id))
        }
    }, [isOpenCart])

    return (
        <>
            <Header />
            <div className='app-container'>
                <Routes>
                    <Route path="/" >
                        <Route index element={<Home />} />
                        <Route path="/chi-tiet-san-pham/:id" element={<ProductDetail />} />
                    </Route>
                    <Route path="*" element={<ErrorPage />} />

                </Routes>
            </div>
            <Footer />
            <Popover
                placement="topLeft"
                trigger={["click"]}
                open={isOpenCart}
                title={<Row style={{ justifyContent: "space-between" }}>
                    <div>Giỏ hàng</div>
                    <div>
                        <button className="deleteall" style={{ border: "none", padding: "5px 15px", backgroundColor: "indianred" }}>Xoá tất cả <DeleteOutlined /> </button>
                    </div>
                </Row>}
                content={account != null ? <Cart accountId={account?.id} dataCart={productsInCart} isLoading={isLoading} /> : <CartWithoutLogin />}
            >
                <FloatButton
                    onClick={() => setIsOpenCart(!isOpenCart)}
                    icon={<ShoppingCartOutlined />}
                />
            </Popover>
        </>
    )

}

export default AppRoutes