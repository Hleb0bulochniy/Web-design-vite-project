import { Button} from "react-bootstrap";
import { useAppDispatch } from '../Redux/Hooks';
import { check } from "../Redux/AuthFetchCheck";

export function Check() {
  const dispatch = useAppDispatch();
  const token = localStorage.getItem('accessToken');
  const handleClick = () => {
    dispatch(
        check()
        
    );
    console.log(token);
    console.log("1");
  };

  return (
    <>
      <Button variant="primary" onClick={handleClick}>
        Check
      </Button>
    </>
  );
}