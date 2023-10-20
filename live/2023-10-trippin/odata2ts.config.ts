import { ConfigFileOptions, EmitModes, Modes } from "@odata2ts/odata2ts";

const config: ConfigFileOptions = {
  services: {
    trippin: {
      sourceUrl: "https://services.odata.org/TripPinRESTierService",
      source: "resource/trippin.xml",
      output: "src/app/models",
      mode: Modes.models,
      emitMode: EmitModes.ts,
    }
  }
}

export default config;
