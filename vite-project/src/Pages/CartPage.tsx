import { useEffect, useState } from "react";
import { productApi } from "../Api/Api";
import { Card2 } from "../Components/Card";
import { useAppDispatch, useAppSelector } from "../Redux/Hooks";
import { SumUpd } from "../Redux/SumFetch";
import { useNavigate } from "react-router-dom";
import { Button } from "react-bootstrap";

export interface webItem {
    Id: number;
    Name: string;
    Price: string;
    Image: string;
    Description: string;
    isBought: boolean;
    numInCart: number;
}


export function CartPage() {
    const dispatch = useAppDispatch();
    const [data, setData] = useState<webItem[]>([]);
    const [refreshKey, setRefreshKey] = useState(true);
    const s = useNavigate();

    useEffect(() => {
        productApi("web2additemForCart").then(res => setData(res.data));
        dispatch(SumUpd());
    }, [refreshKey]);

    const handleBuyClick = async () => {
        try {
            await productApi("Buy");
            setRefreshKey(!refreshKey);
            s("/home");
        } catch (error) {
            console.error("Error fetching2 numInCart:", error);
        }
    };

    const sum = useAppSelector((state) => state.sum.value);
    return (
        <>
            <div className="card-container">
                {data?.map((item: webItem) => (
                    <div key={item.Id} className="card-item">
                        <Card2
                            id={item.Id}
                            name={item.Name}
                            description={item.Description}
                            image={item.Image}
                            price={item.Price}

                        />
                    </div>
                ))}
            </div>
            <h1>Сумма: {sum}</h1>
            <Button variant="primary" onClick={() => {handleBuyClick()}}>
                    Заказать
                </Button>
        </>

    );
}