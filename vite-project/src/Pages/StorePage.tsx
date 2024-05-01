import { useEffect, useState } from "react";
import { productApi } from "../Api/Api";
import Card1 from "../Components/Card";
import { CartButton } from "../Components/CartButton";

export interface webItem {
    Id: number;
    Name: string;
    Price: string;
    Image: string;
    Description: string;
    isBought: boolean;
}

export function StorePage() {
    const [data, setData] = useState<webItem[]>([]);
    

    useEffect(() => {
        productApi("web2additem").then(res => { console.log(res); setData(res.data) })
    }, [])

    const handleBuyClick = (itemId: number) => {
        setData(data.map(item =>
            item.Id === itemId ? { ...item, isBought: !item.isBought } : item
        ));
    };

    return (
        < div className="card-container" >
            {data?.map((item: webItem) => (
                <div key={item.Id} className="card-item">
                    <Card1
                        id={item.Id}
                        name={item.Name}
                        description={item.Description}
                        image={item.Image}
                        price={item.Price}
                        button={<CartButton state={item.isBought} fun={() => handleBuyClick(item.Id)} textBuy={item.Price} textBought='Добавлено!' inCartNum={0} isFavourite={false} />}
                    />
                </div>
            ))
            }
        </div >
    )
}
