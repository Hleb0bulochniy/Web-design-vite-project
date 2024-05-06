import { useEffect, useState } from 'react';
import Card from 'react-bootstrap/Card';
import { productApiGetItem } from '../Api/Api';
import { CartButton } from './CartButton';
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from '../Redux/Hooks';
import { ChangeSum } from '../Redux/SumSlice';



interface CardProps {
    id: number;
    name: string;
    description: string;
    price: string;
    image: string;
}

export interface ItemInUser {
    Id: number;
    UserId: number;
    isInCart: boolean;
    isFavourite: boolean;
    itemInCartNumber: number;
    itemId: number;
}

function Card1({ id, name, description, image, price }: CardProps) {
    const dispatch = useAppDispatch();
    const [data, setData] = useState<ItemInUser>();
    const s = useNavigate();
    const [refreshKey, setRefreshKey] = useState(true);

    useEffect(() => {
        productApiGetItem("getInfoById", id).then(res => setData(res.data));
    }, [refreshKey]);

    const handleBuyClick = async (itemId: number) => {
        try {

            await productApiGetItem("getNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching1 numInCart:", error);
            window.location.href = "/login";
        }
    };

    const handleAddClick = async (itemId: number) => {
        try {
            
            await productApiGetItem("addNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching2 numInCart:", error);
            s("/login");
        }
    };

    const handleRemClick = async (itemId: number) => {
        try {
            
            await productApiGetItem("minusNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching3 numInCart:", error);
            s("/login");
        }
    };
    const itemInCartNumber = data?.itemInCartNumber || 0;
    const priceAsNumber = parseFloat(price);
    const sum: number = priceAsNumber * itemInCartNumber;
    
    return (
        <Card style={{ width: '18rem' }}>
            <Card.Img variant="top" src={image} />
            <Card.Body>
                <Card.Title>{name}</Card.Title>
                <Card.Text>
                    {description}
                </Card.Text>
                <CartButton
                    state={data?.isInCart}
                    rand={false}
                    funBuy={() => handleBuyClick(id)}
                    funAdd={() => handleAddClick(id)}
                    funRem={() => handleRemClick(id)}
                    //fun={() => setData()}
                    textBuy={price}
                    textBought={`В корзине: ${data?.itemInCartNumber}шт(${sum} рублей)`}
                    inCartNum={data?.itemInCartNumber}
                    isFavourite={false}
                    id={id} />
            </Card.Body>
        </Card>
    );
}

export function Card2({ id, name, description, image, price }: CardProps) {
    const [data, setData] = useState<ItemInUser>();
    const s = useNavigate();
    const [refreshKey, setRefreshKey] = useState(true);

    useEffect(() => {
        productApiGetItem("getInfoById", id).then(res => setData(res.data));
    }, [refreshKey]);

    const handleBuyClick = async (itemId: number) => {
        try {
            await productApiGetItem("getNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching1 numInCart:", error);
            window.location.href = "/login";
        }
    };

    const handleAddClick = async (itemId: number) => {
        try {
            await productApiGetItem("addNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching2 numInCart:", error);
            s("/login");
        }
    };

    const handleRemClick = async (itemId: number) => {
        try {
            await productApiGetItem("minusNumInCartById", itemId);
            setRefreshKey(!refreshKey);
        } catch (error) {
            console.error("Error fetching3 numInCart:", error);
            s("/login");
        }
    };

    const itemInCartNumber = data?.itemInCartNumber || 0;
    const priceAsNumber = parseFloat(price);
    const sum: number = priceAsNumber * itemInCartNumber;

    return (
        <Card style={{ width: '18rem' }}>
            <div className="card-container">
                <Card.Img src={image} />
                <Card.Body>
                    <Card.Title>{name}</Card.Title>
                    <Card.Text>
                        {description}
                    </Card.Text>
                    <CartButton
                        state={data?.isInCart}
                        rand={false}
                        funBuy={() => handleBuyClick(id)}
                        funAdd={() => handleAddClick(id)}
                        funRem={() => handleRemClick(id)}
                        textBuy={price}
                        textBought={`В корзине: ${data?.itemInCartNumber}шт(${sum} рублей)`}
                        inCartNum={data?.itemInCartNumber}
                        isFavourite={false}
                        id={id}
                    />
                </Card.Body>
            </div>
        </Card>
    );
}

export default Card1;