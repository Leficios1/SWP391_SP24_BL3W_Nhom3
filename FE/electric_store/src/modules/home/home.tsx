import { Card, Col, FloatButton, Pagination, Popover, Row, Skeleton } from "antd"
import Meta from "antd/es/card/Meta";
import axios from "axios";
import React, { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../config/store";
import { ShoppingCartOutlined } from "@ant-design/icons";


const Home = () => {

    const [product, setProduct] = useState([]); //test only
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate()

    const BannerStyle: React.CSSProperties = {
        margin: "20px 0px"
    }

    const fetchProduct = () => {
        setIsLoading(true)
        axios("https://65387970a543859d1bb17924.mockapi.io/api/v1/products")
            .then((res) => {
                if (res.status === 200) {
                    setIsLoading(false)
                    setProduct(res.data);
                }

            })
    }

    const toDetailProductPage = (id: string) => {
        navigate(`chi-tiet-san-pham/${id}`)
    }

    useEffect(() => {
        fetchProduct()
    }, [])


    return (
        <>
            <Row style={BannerStyle} className="banner">
                <Col span={24}>
                    <img width={"100%"} src="https://mekoong.com/wp-content/uploads/2022/07/DO-DIEN-GIA-DUNG-1.jpg" alt="banner_image" />
                </Col>
            </Row>

            <Row gutter={[50, 50]} className="products">
                {isLoading ? <Skeleton /> :
                    product.map((product: any) => {
                        return (
                            <Col key={product.id} md={6} onClick={() => toDetailProductPage(product.id)}>
                                <Card
                                    hoverable
                                    style={{ width: "100%" }}
                                    cover={<img alt="example" src={product.avatar} />}
                                >
                                    <Meta title={product.name} description={product.description} />
                                </Card>
                            </Col>
                        )
                    })
                }
            </Row>

            <Row style={{ marginTop: "30px" }}>
                <Col style={{ textAlign: "center" }} span={24}>
                    <Pagination defaultCurrent={6} total={50} />
                </Col>
            </Row>


            <Popover
                placement="topLeft"
                trigger={["click"]}
                open={true}
                title="Giỏ hàng"
                content={<div>
                    123123
                </div>}
            >
                <FloatButton

                    icon={<ShoppingCartOutlined />}
                />
            </Popover>
        </>
    )
}

export default Home