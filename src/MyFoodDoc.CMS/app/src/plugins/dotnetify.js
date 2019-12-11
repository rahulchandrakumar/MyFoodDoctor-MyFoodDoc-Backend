import dotnetify from "dotnetify/vue";
import { MessagePackHubProtocol } from '@aspnet/signalr-protocol-msgpack';

dotnetify.hubServerUrl = process.env.VUE_APP_WEB_API_URL;
//dotnetify.hubOptions.connectionBuilder = builder => builder.withHubProtocol(new MessagePackHubProtocol());