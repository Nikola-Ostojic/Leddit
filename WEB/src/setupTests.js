// create-react-app by default expects this file to be located src/setupTests
import { configure } from "enzyme";
import Adapter from "enzyme-adapter-react-16";

configure({ adapter: new Adapter() });
