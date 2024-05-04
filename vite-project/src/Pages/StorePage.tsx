import { useEffect, useState } from "react";
import { productApi } from "../Api/Api"; // Добавляем productApiGetItem
import Card1 from "../Components/Card";
import { useAppSelector } from "../Redux/Hooks";

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
    const sum = useAppSelector((state) => state.sum.value);
    

    return (
        <>
        <div className="card-container">
            {data?.map((item: webItem) => (
                <div key={item.Id} className="card-item">
                    <Card1
                        id={item.Id}
                        name={item.Name}
                        description={item.Description}
                        image={item.Image}
                        price={item.Price}
                    />
                </div>
            ))}
        </div>
        <h1>{sum}</h1>
        </>
        
    );
}