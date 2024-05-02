import { useEffect, useState } from "react";
import { productApi, productApiGetItem } from "../Api/Api"; // Добавляем productApiGetItem
import Card1 from "../Components/Card";
import { CartButton } from "../Components/CartButton";

export interface webItem {
    Id: number;
    Name: string;
    Price: string;
    Image: string;
    Description: string;
    isBought: boolean;
    numInCart: number;
}

export function StorePage() {
    const [data, setData] = useState<webItem[]>([]);

    useEffect(() => {
        productApi("web2additem").then(res => setData(res.data));
    }, []);

    const handleBuyClick = async (itemId: number) => {
        try {
            const response = await productApiGetItem("getNumInCartById", itemId);
            const numInCart = response.data;
    
            setData(data.map(item =>
                item.Id === itemId ? { ...item, isBought: !item.isBought, numInCart } : item
            ));
        } catch (error) {
            console.error("Error fetching numInCart:", error);
        }
    };

    return (
        <div className="card-container">
            {data?.map((item: webItem) => (
                <div key={item.Id} className="card-item">
                    <Card1
                        id={item.Id}
                        name={item.Name}
                        description={item.Description}
                        image={item.Image}
                        price={item.Price}
                        button={<CartButton
                            state={item.isBought}
                            fun={() => handleBuyClick(item.Id)}
                            textBuy={item.Price}
                            textBought={'В корзине:' + item.numInCart}
                            inCartNum={item.numInCart} // Передаем numInCart в компонент CartButton
                            isFavourite={false} 
                            id = {item.Id}/>}
                    />
                </div>
            ))}
        </div>
    );
}